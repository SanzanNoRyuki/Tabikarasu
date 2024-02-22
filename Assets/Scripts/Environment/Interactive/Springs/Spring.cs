using System;
using UnityEngine;
using UnityEngine.Events;
using Utility.Extensions;

namespace Environment.Interactive.Springs
{
    /// <summary>
    /// Bouncy spring.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Spring : MonoBehaviour
    {
        /// <summary>
        /// Minimum <see cref="springForce">spring force</see>.
        /// </summary>
        public const float MinForce = 0f;

        /// <summary>
        /// Maximum <see cref="springForce">spring force</see>.
        /// </summary>
        public const float MaxForce = 5000f;

        [SerializeField]
        [Tooltip("Layers that can bounce from this platform.")]
        private LayerMask bouncable = Physics2D.AllLayers;

        [SerializeField]
        [Range(MinForce, MaxForce)]
        [Tooltip("How much force is applied to an object.")]
        private float springForce = 2000f;

        [SerializeField]
        [Tooltip("Event invoked when object bounces from this platform.")]
        private UnityEvent onBounce;

        /// <summary>
        /// Event invoked when object bounces from this platform.
        /// </summary>
        public event Action OnBounce;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var otherRb = collision.rigidbody;

            if (otherRb.bodyType != RigidbodyType2D.Dynamic)   return;
            if (!bouncable.Contains(otherRb.gameObject.layer)) return;
            if (!collision.FromLocalTop())                     return;
            
            Bounce(otherRb, transform.up);
        }

        private void Bounce(Rigidbody2D otherRb, Vector2 direction)
        {
            otherRb.AddForce(springForce * direction, ForceMode2D.Impulse);
            onBounce.Invoke();
            OnBounce?.Invoke();
        }
    }
}