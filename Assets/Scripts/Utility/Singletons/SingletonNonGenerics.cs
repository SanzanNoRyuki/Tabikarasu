using UnityEngine;

namespace Utility.Singletons
{
    /// <summary>
    /// Singleton parent holding non-generic functionality.
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    public abstract class SingletonNonGenerics : MonoBehaviour
    {
        /// <summary>
        /// Application is quitting flag. Used to prevent new instances from being created during quit.
        /// </summary>
        public static bool ApplicationIsQuitting { get; private set; }

        /// <summary>
        /// Application is quitting.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
    }
}