using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Constants;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Collision2D"/>.
    /// </summary>
    public static class Collision2DExtensions
    {
        // Helper collection.
        // There are usually only 1-2 contacts during collision (though possible range is 1 to infinity).
        private static readonly List<ContactPoint2D> Contacts = new(capacity: 2);

        /// <summary>
        /// Collision occurred from the top of <see cref="Collision2D">otherCollider</see>.
        /// </summary>
        /// <param name="collision">Collision reference.</param>
        /// <returns>True if collision occurred from top. False otherwise.</returns>
        public static bool FromTop(this Collision2D collision)
        {
            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the bottom section of the unit circle.
            return Contacts.Any(contact => contact.normal.y <= -1f * Values.Sin45);
        }

        /// <summary>
        /// <inheritdoc cref="FromTop"/>
        /// Respects object rotation.
        /// </summary>
        /// <param name="collision"><inheritdoc cref="FromTop"/></param>
        /// <returns><inheritdoc cref="FromTop"/></returns>
        public static bool FromLocalTop(this Collision2D collision)
        {
            float transformRotation = collision.otherRigidbody.transform.eulerAngles.z;

            // No modification needed.
            if (Mathf.Approximately(transformRotation, 0f)) return collision.FromTop();

            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            return Contacts.Any(contact => contact.normal.Rotate(0f, 0f, -1f * transformRotation).y <= -1f * Values.Sin45);
        }

        /// <summary>
        /// Collision occurred from the bottom of <see cref="Collision2D">otherCollider</see>.
        /// </summary>è
        /// <param name="collision">Collision reference.</param>
        /// <returns>True if collision occurred from bottom. False otherwise.</returns>
        public static bool FromBottom(this Collision2D collision)
        {
            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the top section of the unit circle.
            return Contacts.Any(contact => contact.normal.y >= Values.Sin45);
        }

        /// <summary>
        /// <inheritdoc cref="FromBottom"/>
        /// Respects object rotation.
        /// </summary>
        /// <param name="collision"><inheritdoc cref="FromBottom"/></param>
        /// <returns><inheritdoc cref="FromBottom"/></returns>
        public static bool FromLocalBottom(this Collision2D collision)
        {
            float transformRotation = collision.otherRigidbody.transform.eulerAngles.z;

            // No modification needed.
            if (Mathf.Approximately(transformRotation, 0f)) return collision.FromBottom();

            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the top section of the unit circle.
            return Contacts.Any(contact => contact.normal.Rotate(0f, 0f, -1f * transformRotation).y >= Values.Sin45);
        }

        /// <summary>
        /// Collision occurred from the left of <see cref="Collision2D">otherCollider</see>.
        /// </summary>
        /// <param name="collision">Collision reference.</param>
        /// <returns>True if collision occurred from left. False otherwise.</returns>
        public static bool FromLeft(this Collision2D collision)
        {
            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the right section of the unit circle.
            return Contacts.Any(contact => contact.normal.x >= Values.Cos45);
        }

        /// <summary>
        /// <inheritdoc cref="FromLeft"/>
        /// Respects object rotation.
        /// </summary>
        /// <param name="collision"><inheritdoc cref="FromLeft"/></param>
        /// <returns><inheritdoc cref="FromLeft"/></returns>
        public static bool FromLocalLeft(this Collision2D collision)
        {
            float transformRotation = collision.otherRigidbody.transform.eulerAngles.z;

            // No modification needed.
            if (Mathf.Approximately(transformRotation, 0f)) return collision.FromLeft();

            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the right section of the unit circle.
            return Contacts.Any(contact => contact.normal.Rotate(0f, 0f, -1f * transformRotation).x >= Values.Cos45);
        }

        /// <summary>
        /// Collision occurred from the right of <see cref="Collision2D">otherCollider</see>.
        /// </summary>
        /// <param name="collision">Collision reference.</param>
        /// <returns>True if collision occurred from right. False otherwise.</returns>
        public static bool FromRight(this Collision2D collision)
        {
            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the left section of the unit circle.
            return Contacts.Any(contact => contact.normal.x <= -1f * Values.Cos45);
        }

        /// <summary>
        /// <inheritdoc cref="FromRight"/>
        /// Respects object rotation.
        /// </summary>
        /// <param name="collision"><inheritdoc cref="FromRight"/></param>
        /// <returns><inheritdoc cref="FromRight"/></returns>
        public static bool FromLocalRight(this Collision2D collision)
        {
            float transformRotation = collision.otherRigidbody.transform.eulerAngles.z;

            // No modification needed.
            if (Mathf.Approximately(transformRotation, 0f)) return collision.FromRight();

            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the left section of the unit circle.
            return Contacts.Any(contact => contact.normal.Rotate(0f, 0f, -1f * transformRotation).x <= -1f * Values.Cos45);
        }

        /// <summary>
        /// Collision occurred from the side of <see cref="Collision2D">otherCollider</see>.
        /// </summary>
        /// <param name="collision">Collision reference.</param>
        /// <returns>True if collision occurred from left or right. False otherwise.</returns>
        public static bool FromSide(this Collision2D collision)
        {
            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the left or right section of the unit circle.
            return Contacts.Any(contact => Mathf.Abs(contact.normal.x) >= Values.Cos45);
        }

        /// <summary>
        /// Collision occurred from the side of <see cref="Collision2D">otherCollider</see>.
        /// </summary>
        /// <param name="collision">Collision reference.</param>
        /// <returns>True if collision occurred from left or right. False otherwise.</returns>
        public static bool FromLocalSide(this Collision2D collision)
        {
            float transformRotation = collision.otherRigidbody.transform.eulerAngles.z;

            // No modification needed.
            if (Mathf.Approximately(transformRotation, 0f)) return collision.FromSide();

            // Reuse helper collection.
            Contacts.Clear();
            collision.GetContacts(Contacts);

            // Normal is in the left section of the unit circle.
            return Contacts.Any(contact => Mathf.Abs(contact.normal.Rotate(0f, 0f, -1f * transformRotation).x) >= Values.Cos45);
        }

	    /// <summary>
        /// Returns the other object in the collision.
        /// </summary>
        /// <param name="collision">Collision reference.</param>
        /// <returns>If the other object has a rigidbody, returns the rigidbody, otherwise returns the collider.</returns>
        public static GameObject Other(this Collision2D collision)
        {
            return collision.rigidbody != null ? collision.rigidbody.gameObject : collision.collider.gameObject;
        }
    }
}