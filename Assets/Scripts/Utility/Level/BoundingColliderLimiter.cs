#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Utility.Level
{
    /// <summary>
    /// Limits bounding collider to cover only a designated area.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PolygonCollider2D))]
    public sealed class BoundingColliderLimiter : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Area to limit the collider to.")]
        private Vector2 area = new(1024f, 1024f);

        private void OnValidate()
        {
            CenterCollider();
            LimitCollider();
        }

        private void OnDrawGizmosSelected()
        {
            CenterCollider();
            LimitCollider();
        
            #if UNITY_EDITOR
            EditorApplication.QueuePlayerLoopUpdate();
            #endif
        }

        private void CenterCollider()
        {
            Transform tf = transform;
            
            tf.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            tf.localScale = Vector3.one;
        }

        private void LimitCollider()
        {
            PolygonCollider2D col = GetComponent<PolygonCollider2D>();
            if (col.points == null) return;

            Vector2[] newPoints = new Vector2[col.points.Length];
            Vector2   halfArea  = area / 2f;
            
            for (int i = 0; i < col.points.Length; i++) newPoints[i] = new Vector2(Mathf.Clamp(col.points[i].x, -halfArea.x, halfArea.x), Mathf.Clamp(col.points[i].y, -halfArea.y, halfArea.y));
            col.points = newPoints;
        }

        [ContextMenu("Reset Collider")]
        private void ResetCollider()
        {
            Vector2[] newPoints = new Vector2[4];
            Vector2   halfArea  = area / 2f;

            newPoints[0] = new Vector2(-halfArea.x, -halfArea.y);
            newPoints[1] = new Vector2(-halfArea.x, halfArea.y);
            newPoints[2] = new Vector2( halfArea.x, halfArea.y);
            newPoints[3] = new Vector2( halfArea.x, -halfArea.y);

            GetComponent<PolygonCollider2D>().points = newPoints;
        }
    }
}