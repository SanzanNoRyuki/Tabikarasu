using System;
using System.Collections;
using UnityEngine;
using Utility.Attributes;

namespace Utility.Physics_Checks
{
    /// <summary>
    /// <see cref="GroundCheck"/> extended with <see cref="CoyoteGrounded"/>.
    /// Allows delayed ground detection.
    /// </summary>
    public class CoyoteGroundCheck : GroundCheck
    {
        [ReadOnly]
        [SerializeField]
        [Tooltip("Object is in grace period.")]
        private bool coyoteGrounded;

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("How long to stay grounded after leaving ground.")]
        private float coyoteTime = 0.2f;
        private WaitForSeconds _coyoteTime;

        private Coroutine _coyoteCountdown;
        
        /// <summary>
        /// Raised when object is no longer even on imaginary ground.
        /// </summary>
        public event Action CoyoteTakenOff;

        /// <summary>
        /// <br/>Object was touching ground in the specified time window.
        /// <br/>See <see cref="coyoteTime">coyote time</see>.
        /// </summary>
        public bool CoyoteGrounded => Grounded || coyoteGrounded;

        private void OnEnable()
        {
            TakenOff += DelayTakingOff;
        }

        private void OnDisable()
        {
            TakenOff -= DelayTakingOff;
        }

        protected override void ResetProperties()
        {
            base.ResetProperties();

            _coyoteTime = new WaitForSeconds(coyoteTime);
        }

        private void DelayTakingOff()
        {
            if (_coyoteCountdown != null) StopCoroutine(_coyoteCountdown);

            _coyoteCountdown = StartCoroutine(CoyoteCountdown());
        }

        private IEnumerator CoyoteCountdown()
        {
            coyoteGrounded = true;
            yield return _coyoteTime;
            coyoteGrounded = false;
            CoyoteTakenOff?.Invoke();
        }
    }
}