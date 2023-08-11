using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;
using Utility.Constants;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Rigidbody2D"/>.
    /// </summary>
    public static class Rigidbody2DExtensions
    {
        // Helper collections.
        // There is usually maximum of 8 contacts on rigidbody (though possible range is 0 to infinity).
        private static readonly List<ContactPoint2D> Contacts = new(capacity: 8);
        // Most objects have 1 collider, but it's possible to have more.
        private static readonly List<Collider2D> Colliders = new(capacity: 1);

        /// <summary>
        /// Get <see cref="Bounds"/> of <see cref="Rigidbody2D"/>.
        /// </summary>
        /// <param name="rb">Rigidbody reference.</param>
        /// <returns>Bounding box containing all colliders attached to rigidbody.</returns>
        public static Bounds GetBounds(this Rigidbody2D rb)
        {
            // Reuse helper collection.
            Colliders.Clear();
            rb.GetAttachedColliders(Colliders);

            var bounds = new Bounds(rb.transform.position, Vector3.zero);
            foreach (var collider in Colliders)
            {
                bounds.Encapsulate(collider.bounds);
            }

            return bounds;
        }

        /// <summary>
        /// Rigidbody has contact from the top.
        /// </summary>
        /// <param name="rb">Rigidbody reference.</param>
        /// <returns>True if rigidbody has contact from the top. False otherwise.</returns>
        public static bool HasContactFromTop(this Rigidbody2D rb)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(Contacts);

            // Normal is in the bottom section of the unit circle.
            return Contacts.Any(contact => contact.normal.y <= -1f * Values.Sin45);
        }

        /// <summary>
        /// <inheritdoc cref="HasContactFromTop(UnityEngine.Rigidbody2D)"/>
        /// </summary>
        /// <param name="rb"><inheritdoc cref="HasContactFromTop(UnityEngine.Rigidbody2D)"/></param>
        /// <param name="filter">Contact filter.</param>
        /// <returns><inheritdoc cref="HasContactFromTop(UnityEngine.Rigidbody2D)"/></returns>
        public static bool HasContactFromTop(this Rigidbody2D rb, ContactFilter2D filter)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(filter, Contacts);

            // Normal is in the bottom section of the unit circle.
            return Contacts.Any(contact => contact.normal.y <= -1f * Values.Sin45);
        }

        /// <summary>
        /// Rigidbody has contact from the bottom.
        /// </summary>
        /// <param name="rb">Rigidbody reference.</param>
        /// <returns>True if rigidbody has contact from the bottom. False otherwise.</returns>
        public static bool HasContactFromBottom(this Rigidbody2D rb)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(Contacts);

            // Normal is in the top section of the unit circle.
            return Contacts.Any(contact => contact.normal.y >= Values.Sin45);
        }

        /// <summary>
        /// <inheritdoc cref="HasContactFromBottom(UnityEngine.Rigidbody2D)"/>
        /// </summary>
        /// <param name="rb"><inheritdoc cref="HasContactFromBottom(UnityEngine.Rigidbody2D)"/></param>
        /// <param name="filter">Contact filter.</param>
        /// <returns><inheritdoc cref="HasContactFromBottom(UnityEngine.Rigidbody2D)"/></returns>
        public static bool HasContactFromBottom(this Rigidbody2D rb, ContactFilter2D filter)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(filter, Contacts);

            // Normal is in the top section of the unit circle.
            return Contacts.Any(contact => contact.normal.y >= Values.Sin45);
        }

        /// <summary>
        /// Rigidbody has contact from the left.
        /// </summary>
        /// <param name="rb">Rigidbody reference.</param>
        /// <returns>True if rigidbody has contact from the left. False otherwise.</returns>
        public static bool HasContactFromLeft(this Rigidbody2D rb)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(Contacts);

            // Normal is in the right section of the unit circle.
            return Contacts.Any(contact => contact.normal.x >= Values.Cos45);
        }

        /// <summary>
        /// <inheritdoc cref="HasContactFromLeft(UnityEngine.Rigidbody2D)"/>
        /// </summary>
        /// <param name="rb"><inheritdoc cref="HasContactFromLeft(UnityEngine.Rigidbody2D)"/></param>
        /// <param name="filter">Contact filter.</param>
        /// <returns><inheritdoc cref="HasContactFromLeft(UnityEngine.Rigidbody2D)"/></returns>
        public static bool HasContactFromLeft(this Rigidbody2D rb, ContactFilter2D filter)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(filter, Contacts);

            // Normal is in the right section of the unit circle.
            return Contacts.Any(contact => contact.normal.x >= Values.Cos45);
        }

        /// <summary>
        /// Rigidbody has contact from the right.
        /// </summary>
        /// <param name="rb">Rigidbody reference.</param>
        /// <returns>True if rigidbody has contact from the right. False otherwise.</returns>
        public static bool HasContactFromRight(this Rigidbody2D rb)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(Contacts);

            // Normal is in the left section of the unit circle.
            return Contacts.Any(contact => contact.normal.x <= -1f * Values.Cos45);
        }

        /// <summary>
        /// <inheritdoc cref="HasContactFromRight(UnityEngine.Rigidbody2D)"/>
        /// </summary>
        /// <param name="rb"><inheritdoc cref="HasContactFromRight(UnityEngine.Rigidbody2D)"/></param>
        /// <param name="filter">Contact filter.</param>
        /// <returns><inheritdoc cref="HasContactFromRight(UnityEngine.Rigidbody2D)"/></returns>
        public static bool HasContactFromRight(this Rigidbody2D rb, ContactFilter2D filter)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(filter, Contacts);

            // Normal is in the left section of the unit circle.
            return Contacts.Any(contact => contact.normal.x <= -1f * Values.Cos45);
        }

        /// <summary>
        /// Rigidbody has contact from any side.
        /// </summary>
        /// <param name="rb">Rigidbody reference.</param>
        /// <returns>True if rigidbody has contact from left of right side. False otherwise.</returns>
        public static bool HasContactFromSide(this Rigidbody2D rb)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(Contacts);

            // Normal is in the left or right section of the unit circle.
            return Contacts.Any(contact => Mathf.Abs(contact.normal.x) >= Values.Cos45);
        }

        /// <summary>
        /// <inheritdoc cref="HasContactFromSide(UnityEngine.Rigidbody2D)"/>
        /// </summary>
        /// <param name="rb"><inheritdoc cref="HasContactFromSide(UnityEngine.Rigidbody2D)"/></param>
        /// <param name="filter">Contact filter.</param>
        /// <returns><inheritdoc cref="HasContactFromSide(UnityEngine.Rigidbody2D)"/></returns>
        public static bool HasContactFromSide(this Rigidbody2D rb, ContactFilter2D filter)
        {
            // Reuse helper collection.
            Contacts.Clear();
            rb.GetContacts(filter, Contacts);

            // Normal is in the left or right section of the unit circle.
            return Contacts.Any(contact => Mathf.Abs(contact.normal.x) >= Values.Cos45);
        }

        /// <summary>
        /// See <see cref="Physics2D.IgnoreCollision(Collider2D, Collider2D, bool)">ignore collision</see>.
        /// </summary>
        /// <param name="rb">Rigidbody reference.</param>
        /// <param name="otherCollider">Collider you want to start or stop ignoring collisions with.</param>
        /// <param name="ignore">Whether or not the collisions between the two colliders should be ignored or not.</param>
        public static void IgnoreCollision(this Rigidbody2D rb, Collider2D otherCollider, bool ignore)
        {
            // Reuse helper collection.
            Colliders.Clear();
            rb.GetAttachedColliders(Colliders);

            foreach (var collider in Colliders)
            {
                Physics2D.IgnoreCollision(collider, otherCollider, ignore);
            }
        }

        /// <summary>
        /// See <see cref="Physics2D.IgnoreCollision(Collider2D, Collider2D, bool)">ignore collision</see>.
        /// </summary>
        /// <param name="rb">Rigidbody reference.</param>
        /// <param name="ignore">Whether or not the collisions between the two colliders should be ignored or not.</param>
        public static void IgnoreCollision(this Rigidbody2D rb, bool ignore)
        {
            // Reuse helper collection.
            Colliders.Clear();
            if (ignore)
            {
                rb.GetComponentsInChildren(Colliders);
            }
            else
            {
                rb.GetAttachedColliders(Colliders);
            }

            foreach (var collider in Colliders)
            {
                collider.enabled = ignore;
            }
        }
    }
}