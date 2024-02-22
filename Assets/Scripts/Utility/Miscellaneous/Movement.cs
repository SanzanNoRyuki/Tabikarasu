using System;
using UnityEngine;

namespace Miscellaneous.Movement
{
    /// <summary>
    /// Base class for all movement scripts.
    /// </summary>
    public abstract class Movement : MonoBehaviour
    {
        /// <summary>
        /// Minimum movement speed.
        /// </summary>
        public const float MinSpeed = 0f;

        /// <summary>
        /// Maximum movement speed.
        /// </summary>
        public const float MaxSpeed = 100f;

        /// <summary>
        /// Minimum acceleration.
        /// </summary>
        /// <remarks>
        /// Might not be used by all movement scripts.
        /// </remarks>
        public const float MinAcceleration = 0.01f;

        /// <summary>
        /// Maximum acceleration.
        /// </summary>
        /// <remarks>
        /// Might not be used by all movement scripts.
        /// </remarks>
        public const float MaxAcceleration = 10f;

        /// <summary>
        /// Minimum speed power.
        /// </summary>
        /// <remarks>
        /// Might not be used by all movement scripts.
        /// </remarks>
        public const float MinSpeedPower = 0.01f;

        /// <summary>
        /// Maximum speed power.
        /// </summary>
        /// <remarks>
        /// Might not be used by all movement scripts.
        /// </remarks>
        public const float MaxSpeedPower = 1f;

        /// <summary>
        /// Minimum friction.
        /// </summary>
        /// <remarks>
        /// Might not be used by all movement scripts.
        /// </remarks>
        public const float MinFriction = 0f;

        /// <summary>
        /// Maximum friction.
        /// </summary>
        /// <remarks>
        /// Might not be used by all movement scripts.
        /// </remarks>
        public const float MaxFriction = 1f;

        /*
        [SerializeField]
        [Range(MinSpeed, MaxSpeed)]
        [Tooltip("Speed of the object.")]
        private float speed = 5f;


        [ReadOnly]
        [SerializeField]
        [Tooltip("Speed modifier used by other scripts.")]
        private float speedModifier = 1f;


        public float Speed => speed  * speedModifier;*/


        protected virtual void OnEnable()
        {
            EnforceUniqueness();
        }
        
        private void EnforceUniqueness()
        {
            foreach (var movement in GetComponents<Movement>())
            {
                if (movement.enabled && movement != this)
                {
                    Debug.LogWarning("Only one movement script can be enabled at a time.", this);
                    movement.enabled = false;
                }
            }
        }

        public enum JumpType
        {
            Grounded,
            MidAir,
        }
    }
}
