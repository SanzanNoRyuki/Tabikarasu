using UnityEngine;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="LayerMask"/>.
    /// </summary>
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// Checks if mask contains specified layer.
        /// </summary>
        /// <param name="layerMask">Layer mask reference.</param>
        /// <param name="layer">Layer to check.</param>
        /// <returns>True if the checked layer is included in the mask. False otherwise.</returns>
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        /// <summary>
        /// Allows only one layer to be selected at a time.
        /// </summary>
        /// <param name="layerMask">Layer mask reference.</param>
        /// <returns>Layer mask with first layer only.</returns>
        public static LayerMask Unique(this LayerMask layerMask)
        {
            int bitIndex = 0;
            while (layerMask > 0)
            {
                if ((layerMask & 1) == 1) return 1 << bitIndex;
                layerMask >>= 1;
                bitIndex++;
            }
            return layerMask;
        }
    }
}