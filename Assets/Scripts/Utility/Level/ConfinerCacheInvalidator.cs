using Cinemachine;
using UnityEngine;

namespace Utility.Level
{
    /// <summary>
    /// Invalidates cache on related <see cref="CinemachineConfiner2D"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CinemachineConfiner2D))]
    public sealed class ConfinerCacheInvalidator : MonoBehaviour, IRebuildable
    {
        /// <summary>
        /// <inheritdoc cref="ConfinerCacheInvalidator"/>
        /// </summary>
        [ContextMenu("Invalidate Cache")]
        public void InvalidateCache()
        {
            GetComponent<CinemachineConfiner2D>().InvalidateCache();
        }

        /// <summary>
        /// <inheritdoc cref="ConfinerCacheInvalidator"/>
        /// </summary>
        public void Rebuild()
        {
            InvalidateCache();
        }
    }
}