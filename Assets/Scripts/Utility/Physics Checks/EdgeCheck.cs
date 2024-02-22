using System;
using System.Collections;
using UnityEngine;
using Utility.Attributes;
using Utility.Extensions;

namespace Utility.Physics_Checks
{
    /// <summary>
    /// Checks if object is faces cliff ledge.
    /// To give valid result, Offset must be updated before this check
    /// and after every <see cref="Collider2D"/> modification (addition/removal/resizing).
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(GroundCheck))]
    public class EdgeCheck : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        [Tooltip("Object is on top of ledge.")]
        private bool onLedge;

        [Range(0f, 0.5f)]
        [SerializeField]
        private float dizzyTime = 0.1f;

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("How deep to look for the cliff side.")]
        private float checkDistance = 0.5f;

        private Rigidbody2D _rb;
        private GroundCheck _groundCheck;

        private LayerMask   _ground;
        private Vector2     _cornerOffset;

        /// <summary>
        /// Raised when object starts facing cliff edge.
        /// </summary>
        public event Action GotToLedge;

        /// <summary>
        /// Raised when object stops facing cliff edge.
        /// </summary>
        public event Action GotAwayFromLedge;

        /// <summary>
        /// Object is on top of ledge.
        /// </summary>
        public bool OnLedge => onLedge;

        private void Awake()
        {
            _rb          = GetComponent<Rigidbody2D>();
            _groundCheck = GetComponent<GroundCheck>();

            UpdateOffset();
        }

        private void Start()
        {
            _ground = _groundCheck.Ground;
        }

        private void OnEnable()
        {
            _groundCheck.Landed                 += StartCStartCoroutine;

        }

        private void OnDisable()
        {
            _groundCheck.Landed                 -= StartCStartCoroutine;
        }

        private void StartCStartCoroutine()
        {
            StartCoroutine(DizzyTime());
        }

    private void FixedUpdate()
    {
        var currentOnLedge = IsOnLedge();

            if (currentOnLedge != onLedge)
            {
                onLedge = currentOnLedge;

                if (currentOnLedge)
                {
                    GotToLedge?.Invoke();
                }
                else
                {
                    GotAwayFromLedge?.Invoke();
                }
            }
    }

        private WaitForSeconds _dizzyTime;
        
        private void OnValidate()
        {
            _dizzyTime = new WaitForSeconds(dizzyTime);
        }

        private void Reset()
        {
            UpdateOffset();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var cornerOffset = _cornerOffset;
            if (transform.FacingLeft2D()) cornerOffset.x *= -1f;


            var cornerPoint = (Vector2) transform.position + cornerOffset;

            Gizmos.color = OnLedge ? Color.red : Color.green;
            Gizmos.DrawRay(cornerPoint, Vector3.down * checkDistance);
        }
#endif


        private bool _landed;
        private IEnumerator DizzyTime()
        {
            _landed = true;
            yield return new WaitForSeconds(dizzyTime);
            _landed = false;
        }

        private IEnumerator DizzyTime(float seconds)
        {
            _landed = true;
            yield return new WaitForSeconds(seconds);
            _landed = false;
        }

        

        public bool IsOnLedge()
        {
            // Can't be on the ledge if airborne
            if (!_groundCheck.Grounded || _landed) return false;

            //var cornerOffset = transform.FacingRight2D() ? _cornerOffset : new Vector2(-1f * _cornerOffset.x, _cornerOffset.y);

            var cornerOffset                             = _cornerOffset;
            if (transform.FacingLeft2D()) cornerOffset.x *= -1f;
            var cornerPoint                              = (Vector2) transform.position + cornerOffset;

            var hit =  Physics2D.Raycast(cornerPoint, Vector2.down, checkDistance, _ground);

            return hit.collider == null;
        }






        public void Dizzy(float seconds)
        {
            StartCoroutine(DizzyTime(seconds));
        }





        [ContextMenu("Update area containing all active colliders.")]
        public void UpdateOffset()
        {
            if (transform.FacingRight2D())
            {
                UpdateBottomRightOffset();
            }
            else
            {
                transform.FlipX();
                UpdateBottomRightOffset();
                transform.FlipX();
            }
        }

        // Set offset to bottom right corner
        private void UpdateBottomRightOffset()
        {
            var bounds = GetComponent<Rigidbody2D>().GetBounds();

            var transformCenterOffset = new Vector2(bounds.center.x - transform.position.x, bounds.center.y - transform.position.y);
            _cornerOffset = new Vector2(transformCenterOffset.x + bounds.extents.x, transformCenterOffset.y - bounds.extents.y);
        }
    }
}