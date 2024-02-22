#if UNITY_EDITOR
using UnityEngine;

namespace Editor.Utility
{
    /// <summary>
    /// Automatically scale particle system shape with it's parent scale.
    /// </summary>
    /// <remarks>
    /// Editor only script.
    /// </remarks>
    [ExecuteInEditMode]
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemScaler : MonoBehaviour {

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Makes particle system X dimension smaller by this number than it's parent.")]
        private float ignoredXEdges = 0.25f;

        private Vector3 _previousScale;

        private void Update()
        {
            Vector3 lossyScale = transform.lossyScale;
            lossyScale = new Vector3(lossyScale.x - ignoredXEdges, lossyScale.y, lossyScale.z);

            if (_previousScale == lossyScale) return;
            _previousScale = lossyScale;

            ParticleSystem.ShapeModule shape = GetComponent<ParticleSystem>().shape;
            shape.scale = new Vector3(lossyScale.x, lossyScale.y, lossyScale.z);
        }
    }
}
#endif