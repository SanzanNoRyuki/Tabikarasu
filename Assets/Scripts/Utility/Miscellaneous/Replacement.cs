using UnityEngine;

namespace Utility.Miscellaneous
{
    /// <summary>
    /// Replaces game object with the replacement.
    /// </summary>
    public class Replacement : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Replaces game object.")]
        private GameObject replacement;

        /// <summary>
        /// <inheritdoc cref="Replacement"/>
        /// </summary>
        [ContextMenu("Replace")]
        public void Replace()
        {
            var current = transform;
            Instantiate(replacement, current.position, current.rotation, current.parent);
            Destroy(gameObject);
        }
    }
}