using System;
using UnityEngine;
using UnityEngine.Events;
using Utility.Attributes;

namespace Environment.Decorations
{
    /// <summary>
    /// Corpse of a dead entity.
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    public class Corpse : MonoBehaviour
    {
        [Fixed]
        [SerializeField]
        [Tooltip("Corpse own corpse.")]
        private Corpse corpse;
        private Corpse _corpse;

        [SerializeField]
        [Tooltip("Corpse got spawned.")]
        private UnityEvent spawned;
        
        [SerializeField]
        [Tooltip("Corpse got despawned.")]
        private UnityEvent despawned;

        /// <summary>
        /// Corpse got spawned.
        /// </summary>
        public event Action Spawned;

        /// <summary>
        /// Corpse got despawned.
        /// </summary>
        public event Action Despawned;

        private void Start()
        {
            if (corpse != null)
            {
                _corpse = Instantiate(corpse);
                _corpse.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Spawns corpse at passed position.
        /// </summary>
        /// <param name="position">Position where to place the corpse.</param>
        /// <param name="rotation">Placed corpse rotation.</param>
        public void Spawn(Vector3 position, Quaternion rotation)
        {
            if (_corpse != null) _corpse.Despawn();

            transform.SetPositionAndRotation(position, rotation);
            gameObject.SetActive(true);
            spawned.Invoke();
            Spawned?.Invoke();
        }

        /// <summary>
        /// Despawns corpse.
        /// </summary>
        public void Despawn()
        {
            Transform tf = transform;
            if (_corpse != null) _corpse.Spawn(tf.position, tf.rotation);

            gameObject.SetActive(false);
            despawned.Invoke();
            Despawned?.Invoke();
        }

        [ContextMenu("Spawn")]
        private void SpawnContextMenu()
        {
            Spawn(transform.position, Quaternion.identity);
        }

        [ContextMenu("Despawn")]
        private void DespawnContextMenu()
        {
            Despawn();
        }
    }
}
