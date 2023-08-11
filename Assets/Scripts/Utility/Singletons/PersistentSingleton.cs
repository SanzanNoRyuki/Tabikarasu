using UnityEngine;

namespace Utility.Singletons
{
    /// <summary>
    /// Persistent singleton component.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="Singleton{T}"/></typeparam>
    public abstract class PersistentSingleton<T> : Singleton<T> where T : Component
    {
        protected override void Awake()
        {
            base.Awake();

            // Make singleton persistent.
            DontDestroyOnLoad(this);
        }
    }
}