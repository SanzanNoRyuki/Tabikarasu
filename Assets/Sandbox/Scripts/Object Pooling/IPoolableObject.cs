using System;
using UnityEngine;

namespace Utility.Object_Pooling
{
    /// <summary>
    /// Object suitable for <see cref="ObjectPool">pooling</see>.
    /// </summary>
    public interface IPoolableObject
    {
        /// <summary>
        /// Event invoked during <see cref="Spawn()">spawning</see>.
        /// </summary>
        public event Action Spawned;

        /// <summary>
        /// Spawns the object.
        /// Still has to be <see cref="GameObject.SetActive">activated</see>
        /// and <see cref="Transform.SetPositionAndRotation">positioned</see>.
        /// </summary>
        /// <param name="lifetime">
        /// <inheritdoc cref="Lifetime" path="/summary"/>
        /// </param>
        public void Spawn();

        /// <summary>
        /// <see cref="ObjectPool.DespawnAtTheEndOfFrame">Despawns object at the end of frame.</see>
        /// </summary>
        public void Despawn();
    }
    
}