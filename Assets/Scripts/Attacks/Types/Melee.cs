using System;
using UnityEngine;
using Utility.Extensions;

namespace Attacks.Abilities
{
    /// <summary>
    /// Attack attached directly to object to cause him recognizing hits on collisions.
    /// </summary>
    public class Melee : Attack
    {
        /// <summary>
        /// <inheritdoc cref="Attack.OnHit"/>
        /// </summary>
        public override event Action<GameObject, Vector2> OnHit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsValidTarget(collision.collider, out GameObject go)) OnHit?.Invoke(go, collision.GetContact(0).normal);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsValidTarget(other, out GameObject go)) OnHit?.Invoke(go, transform.DirectionTo(other.transform));
        }
    }
}