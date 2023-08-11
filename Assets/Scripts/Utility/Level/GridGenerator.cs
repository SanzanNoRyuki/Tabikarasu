using UnityEngine;

namespace Utility.Level
{
    /// <summary>
    /// Regenerates <see cref="AstarPath"/> grid to <see cref="PolygonCollider2D"/> bounds.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AstarPath))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public sealed class GridGenerator : MonoBehaviour, IRebuildable
    {
        /// <summary>
        /// <inheritdoc cref="GridGenerator"/>
        /// </summary>
        [ContextMenu("Regenerate Grid")]
        public void RegenerateGrid()
        {
            AstarPath aStarPath = GetComponent<AstarPath>();
            Bounds    bounds    = GetComponent<PolygonCollider2D>().bounds;

            aStarPath.data.gridGraph.center = bounds.center;
            aStarPath.data.gridGraph.SetDimensions(Mathf.CeilToInt(bounds.size.x), Mathf.CeilToInt(bounds.size.y), aStarPath.data.gridGraph.nodeSize);
            aStarPath.Scan();
        }

        /// <summary>
        /// <inheritdoc cref="GridGenerator"/>
        /// </summary>
        public void Rebuild()
        {
            RegenerateGrid();
        }
    }
}