using UnityEngine;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Vector2"/>.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns rotated vector according to given angles.
        /// </summary>
        /// <param name="vector">Original vector.</param>
        /// <param name="x">X axis rotation in degrees.</param>
        /// <param name="y">Y axis rotation in degrees.</param>
        /// <param name="z">Z axis rotation in degrees.</param>
        /// <returns>New vector rotated according to given parameters.</returns>
        public static Vector2 Rotate(this Vector2 vector, float x = 0f, float y = 0f, float z = 0f)
        {
            return Quaternion.Euler(x, y, z) * vector;
        }
    }
}