using UnityEngine;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Collider2D"/>.
    /// </summary>
    public static class Collider2DExtensions
    {
        /// <summary>
        /// Returns the other object in the collision.
        /// </summary>
        /// <param name="collider">Collider reference.</param>
        /// <returns>If the other object has a rigidbody, returns the rigidbody, otherwise returns this collider.</returns>
        public static GameObject Other(this Collider2D collider)
        {
            return collider.attachedRigidbody != null ? collider.attachedRigidbody.gameObject : collider.gameObject;
        }
    }
}