using System;
using UnityEngine;
using UnityEngine.Events;
using Utility.Extensions;

namespace Attacks.Projectiles
{
    /// <summary>
    /// Destroys projectiles it comes in contact with.
    /// </summary>
    [DisallowMultipleComponent]
    public class ProjectileDestroyer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Layers that can be destroyed by this object.")]
        private LayerMask targets = Physics2D.AllLayers;
        
        [SerializeField]
        [Tooltip("Event raised on projectile destruction.")]
        private UnityEvent projectileDestroyed;

        /// <summary>
        /// Event raised on projectile destruction.
        /// </summary>
        public event Action ProjectileDestroyed;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            DestroyProjectiles(collision.Other());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DestroyProjectiles(other.Other());
        }

        private void DestroyProjectiles(GameObject target)
        {
            if (!targets.Contains(target.gameObject.layer)) return;
            if (!target.TryGetComponent(out Projectile projectile)) return;

            projectile.Despawn();
            projectileDestroyed?.Invoke();
            ProjectileDestroyed?.Invoke();
        }
    }
}