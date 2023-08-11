using UnityEngine;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Transform"/>.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Checks if transform is facing right.
        /// </summary>
        /// <remarks>
        /// Works only if every imported asset faces right by default.
        /// </remarks>
        /// <param name="transform">Transform reference.</param>
        /// <returns>True if transform is not flipped by rotating around y axis. False otherwise.</returns>
        public static bool FacingRight2D(this Transform transform)
        {
            return transform.eulerAngles.y is <= 90f or > 270f;
        }

        /// <summary>
        /// Checks if transform is facing left.
        /// </summary>
        /// <remarks>
        /// Works only if every imported asset faces right by default.
        /// </remarks>
        /// <param name="transform">Transform reference.</param>
        /// <returns>True if transform is flipped by rotating around y axis. False otherwise.</returns>
        public static bool FacingLeft2D(this Transform transform)
        {
            return transform.eulerAngles.y is > 90f and <= 270f;
        }

        /// <summary>
        /// Flips transform around y axis.
        /// </summary>
        /// <param name="transform">Transform reference.</param>
        public static void FlipX(this Transform transform)
        {
            transform.Rotate(0f, 180f, 0f);
        }

        /// <summary>
        /// Gets direction from this to other transform.
        /// </summary>
        /// <param name="transform">Transform reference.</param>
        /// <param name="other">Other transform.</param>
        /// <returns>Direction towards other transform.</returns>
        public static Vector3 DirectionTo(this Transform transform, Transform other)
        {
            return (other.position - transform.position).normalized;
        }

        /// <summary>
        /// Transform is facing another one.
        /// </summary>
        /// <param name="transform">Transform reference.</param>
        /// <param name="other">Other transform.</param>
        /// <returns>True if other is in front of transform. False otherwise.</returns>
        public static bool Facing(this Transform transform, Transform other)
        {
            return Vector2.Dot(transform.right, transform.DirectionTo(other).normalized) > 0f;
        }
    }
}