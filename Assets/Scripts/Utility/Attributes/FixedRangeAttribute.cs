using UnityEngine;

namespace Utility.Attributes
{
    /// <summary>
    /// Allows range edits in editor mode only.
    /// </summary>
    public class FixedRangeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Maximum range value.
        /// </summary>
        public readonly float Max;

        /// <summary>
        /// Minimum range value.
        /// </summary>
        public readonly float Min;

        /// <summary>
        /// <inheritdoc cref="FixedRangeAttribute"/>
        /// </summary>
        /// <param name="min">
        /// <inheritdoc cref="Min" path="//summary"/>
        /// </param>
        /// <param name="max">
        /// <inheritdoc cref="Max" path="//summary"/>
        /// </param>
        public FixedRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}