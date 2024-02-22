using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Attributes;

namespace Utility.Object_Pooling
{
    /// <summary>
    /// Pool of game objects.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        /// <summary>
        /// Minimum pool size.
        /// </summary>
        public const uint MinPoolSize = 1;

        /// <summary>
        /// Maximum pool size.
        /// </summary>
        public const uint MaxPoolSize = 500;

        // Actual pool
        private readonly Queue<GameObject> _pool = new();

        [SerializeField]
        [Tooltip("Pooled object prefab.")]
        private GameObject prefab;

        [FixedRange(MinPoolSize, MaxPoolSize)]
        [SerializeField]
        [Tooltip("Size of object pool.")]
        private int size = 1;

        /// <summary>
        /// Size of object pool.
        /// </summary>
        public int Size => size;

        private void Awake()
        {
            // Populate object pool
            for (var i = 0; i < size; i++)
            {
                var clone = Instantiate(prefab);
                clone.SetActive(false);
                _pool.Enqueue(clone);
            }
        }

        /// <summary>
        /// <inheritdoc cref="IPoolableObject.Despawn" />
        /// </summary>
        /// <param name="obj">Object to despawn.</param>
        public static void Despawn(IPoolableObject obj)
        {
            obj.Despawn();
        }

        /// <summary>
        /// <inheritdoc cref="DespawnAtTheEndOfFrame" />
        /// </summary>
        /// <param name="obj">
        /// <inheritdoc cref="DespawnAtTheEndOfFrame" />
        /// </param>
        public void Despawn(GameObject obj)
        {
            StartCoroutine(DespawnAtTheEndOfFrame(obj));
        }

        /// <summary>
        /// Despawns object at the end of frame.
        /// </summary>
        /// <param name="obj">Object to despawn.</param>
        /// <returns>IEnumerator used by coroutine.</returns>
        public static IEnumerator DespawnAtTheEndOfFrame(GameObject obj)
        {
            yield return new WaitForEndOfFrame();

            obj.SetActive(false);
        }

        /// <summary>
        /// Despawns object after delay in seconds.
        /// </summary>
        /// <param name="obj">
        /// <inheritdoc cref="Despawn(IPoolableObject)" />
        /// </param>
        /// <param name="delay">Despawn delay in seconds.</param>
        /// <returns>IEnumerator used by coroutine.</returns>
        public static IEnumerator DelayedDespawn(IPoolableObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            yield return new WaitForEndOfFrame();

            Despawn(obj);
        }

        /// <summary>
        /// Despawns object after delay in seconds.
        /// </summary>
        /// <param name="obj">
        /// <inheritdoc cref="Despawn(UnityEngine.GameObject)" />
        /// </param>
        /// <param name="delay">Despawn delay in seconds.</param>
        /// <returns>IEnumerator used by coroutine.</returns>
        public IEnumerator DelayedDespawn(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            yield return new WaitForEndOfFrame();

            Despawn(obj);
        }

        /// <summary>
        /// Spawn object from pool.
        /// </summary>
        /// <param name="position">Position of spawned object.</param>
        /// <param name="rotation">Rotation of spawned object.</param>
        /// <returns>Spawned object.</returns>
        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            // Obtain object & return it to the end of queue
            var obj = _pool.Dequeue();
            _pool.Enqueue(obj);

            // Spawn
            foreach (var poolableObject in obj.GetComponents<IPoolableObject>())
            {
                poolableObject.Spawn();
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);

            return obj;
        }
    }
}