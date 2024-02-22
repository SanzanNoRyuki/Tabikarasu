using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utility.Level
{
    /// <summary>
    /// <see cref="IRebuildable">Rebuilds</see> components of the level.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LevelBuilder : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Rebuild on start.")]
        private bool rebuildOnStart = true;

        private void Start()
        {
            if (rebuildOnStart) Rebuild();
        }

        /// <summary>
        /// Rebuilds <see cref="IRebuildable"/> components in the scene.
        /// </summary>
        [ContextMenu("Rebuild")]
        public void Rebuild()
        {
            FindObjectsOfType<Component>().OfType<IRebuildable>().ToList().ForEach(rebuildable => rebuildable.Rebuild());
        }

        /// <summary>
        /// Resets all tilemaps in the scene.
        /// </summary>
        [ContextMenu("Clear Tilemaps")]
        public void ClearTilemaps()
        {
            FindObjectsOfType<Tilemap>(true).ToList().ForEach(tilemap => tilemap.ClearAllTiles());
        }
    }
}