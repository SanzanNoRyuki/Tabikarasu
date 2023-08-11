using System;
using System.Collections;
using Input_System.Game_Controller;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility.Attributes;
using Utility.Save_System;
using Utility.Save_System.Data;
using Utility.Singletons;

namespace Utility.Scenes
{
    /// <summary>
    /// Manager of scene transitions.
    /// </summary>
    public class SceneManager : PersistentSingleton<SceneManager>, IPersistentDataHandler
    {
        [FixedRange(0f, 5f)]
        [SerializeField]
        [Tooltip("Transition fade in/out duration.")]
        private float transitionDuration = 1f;
        
        private static int _currentLevel;
        
        private static Coroutine _sceneChange;
        
        private                 WaitForSecondsRealtime _transitionDuration;
        private static readonly WaitForEndOfFrame      WaitForEndOfFrame = new();
        
        /// <summary>
        /// Transition duration.
        /// </summary>
        public static float TransitionDuration => Instance != null ? Instance.transitionDuration : 0f;

        /// <summary>
        ///  Event invoked before scene changes. Used to enable/disable objects for transition.
        /// </summary>
        public static event Action PreSceneChange;

        /// <summary>
        /// Scene just changed. Equivalent to <see cref="UnityEngine.SceneManagement.SceneManager.activeSceneChanged"/>.
        /// </summary>
        public static event Action SceneChanged;

        /// <summary>
        /// Scene changed and should be completely loaded. Used to enable/disable objects for transition.
        /// </summary>
        public static event Action PostSceneChange;

        protected override void Awake()
        {
            base.Awake();
        
            _transitionDuration = new WaitForSecondsRealtime(transitionDuration);
        }

        private void OnEnable()
        {
            GameController.Subscribe(GameController.Actions.Pause, OnPause);
        }

        private void OnDisable()
        {
            GameController.Unsubscribe(GameController.Actions.Pause, OnPause);
        }

        /// <summary>
        /// Load specified level.
        /// </summary>
        /// <param name="index">Index of level to load.</param>
        public static void Load(int index)
        {
            if (Instance == null) return;

            if (_sceneChange != null) Instance.StopCoroutine(_sceneChange);
            _sceneChange = Instance.StartCoroutine(LoadScene(index));
        }

        /// <summary>
        /// Load next level.
        /// </summary>
        public static void Next()
        {
            Load(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }

        public static void Saved()
        {
            Load(_currentLevel);
        }

        /// <summary>
        /// Restart current level.
        /// </summary>
        public static void Restart()
        {
            Load(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Quit application.
        /// </summary>
        public static void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        
        public void Save(ref GameData gameData)
        { 
            gameData.Level = _currentLevel;
        }

        public void Load(GameData gameData)
        {
            _currentLevel = gameData.Level;
        }

        [ContextMenu("Next")]
        private void NextContextMenu()
        {
            Next();
        }

        [ContextMenu("Saved")]
        private void SavedContextMenu()
        {
            Saved();
        }

        [ContextMenu("Restart")]
        private void RestartContextMenu()
        {
            Restart();
        }

        private static IEnumerator LoadScene(int index)
        {
            // Old Scene:
            PreSceneChange?.Invoke();
            yield return Instance._transitionDuration;
            _currentLevel = index;

            // Transition:
            UnityEngine.SceneManagement.SceneManager.LoadScene(index >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings ? 0 : index);
            yield return WaitForEndOfFrame;
            
            // New Scene:
            SceneChanged?.Invoke();
            yield return Instance._transitionDuration;
            PostSceneChange?.Invoke();
        }

        private void OnPause(InputAction.CallbackContext obj)
        {
            Quit();
        }
    }
}