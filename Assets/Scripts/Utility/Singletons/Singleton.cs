using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility.Singletons
{
    /// <summary>
    /// Singleton component.
    /// </summary>
    /// <typeparam name="T">Singleton underlying type.</typeparam>
    public abstract class Singleton<T> : SingletonNonGenerics where T : Component
    {
        private static T _instance;

        /// <summary>
        /// Common instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                // Already instantiated.
                if (_instance != null) return _instance;

                // Assign existing instance.
                _instance = FindObjectOfType<T>(true);

                // Application is ending so new instance cannot be created.
                if (ApplicationIsQuitting) return _instance;
                
                // Create a new instance.
                if (_instance == null) _instance = new GameObject($"Dynamic{typeof(T).Name}").AddComponent<T>();

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance                  =  this as T;
            }
            else if (_instance != this)
            {
                Debug.Log($"Multiple instances of singleton {typeof(T).Name} have been detected. {gameObject.name} instance was removed.", this);
                Destroy(gameObject);
            }

            SceneManager.sceneLoaded   += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        protected virtual void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }

        protected virtual void OnSceneUnloaded(Scene scene) { }
    }
}
