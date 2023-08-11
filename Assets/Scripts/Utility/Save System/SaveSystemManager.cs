using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Attributes;
using Utility.Save_System.Data;
using Utility.Singletons;

namespace Utility.Save_System
{
    /// <summary>
    /// Persistent game data  operations interface.
    /// </summary>
    public sealed class SaveSystemManager : PersistentSingleton<SaveSystemManager>
    {
        [SerializeField]
        private bool savingProgress = true;

        [Fixed]
        [SerializeField]
        private string dataName = "Game.data";

        [SerializeField]
        [Tooltip("Current saved game data.")]
        private GameData gameData;

        private        FileDataHandler              _dataHandler;
        private static List<IPersistentDataHandler> handlers;

        protected override void Awake()
        {
            base.Awake();

            _dataHandler = new FileDataHandler(Application.persistentDataPath, dataName);
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            if (!savingProgress)
            {
                NewGame();
                SaveGame();
            }
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            base.OnSceneLoaded(scene, mode);

            handlers = GetDataHandlers();

            LoadGameInternal();
        }

        protected override void OnSceneUnloaded(Scene scene)
        {
            base.OnSceneUnloaded(scene);


            SaveGameInternal();
        }

        private static List<IPersistentDataHandler> GetDataHandlers()
        {
            return FindObjectsOfType<Component>(true).OfType<IPersistentDataHandler>().ToList();
        }

        public static void LoadGame()
        {
            if (Instance == null) return;
            Instance.LoadGameInternal();
        }

        public static void SaveGame()
        {
            if (Instance == null) return;
            Instance.SaveGameInternal();
        }

        public static void NewGame()
        {
            if (Instance == null) return;
            Instance.NewGameInternal();
        }

        public static void ResetProgress()
        {
            if (Instance == null) return;
            Instance.ResetProgressInternal();
        }

        private void NewGameInternal()
        {
            Instance.gameData = new GameData();
        }

        private void LoadGameInternal()
        {
            gameData = _dataHandler.Load();

            if (Instance.gameData == null)
            {
                Debug.Log("No data was found. Initializing with defaults.");
                NewGameInternal();
            }

            foreach (IPersistentDataHandler handler in handlers)
            {
                handler.Load(Instance.gameData);
            }
        }


        /// <summary>
        /// Updates the game data.
        /// </summary>
        private void SaveGameInternal()
        {
            foreach (IPersistentDataHandler handler in handlers)
            {
                handler.Save(ref Instance.gameData);
            }

            _dataHandler.Save(Instance.gameData);
        }

        private void ResetProgressInternal()
        {
            NewGameInternal();
            _dataHandler.Save(Instance.gameData);
        }
    }
}