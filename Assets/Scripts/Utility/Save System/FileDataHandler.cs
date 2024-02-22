using System;
using System.IO;
using UnityEngine;
using Utility.Save_System.Data;

namespace Utility.Save_System
{
    /// <summary>
    /// Save data JSON file handler.
    /// </summary>
    public class FileDataHandler
    {
        private readonly string _dataPath;

        private readonly string _dataName;

        /// <summary>
        /// <inheritdoc cref="FileDataHandler"/>
        /// </summary>
        /// <param name="dataPath">Path to data file.</param>
        /// <param name="dataName">Data file name.</param>
        public FileDataHandler(string dataPath, string dataName)
        {
            _dataPath = dataPath;
            _dataName = dataName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GameData Load()
        {
            string fullPath = Path.Combine(_dataPath, _dataName);
            GameData gameData = null;


            if (File.Exists(fullPath))
            {
                try
                {
                    using FileStream   stream     = new(fullPath, FileMode.Open);
                    using StreamReader reader     = new(stream);
                    string             dataToLoad = reader.ReadToEnd();
                    

                    gameData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading data from {fullPath}:\n{e}");
                }
            }
            return gameData;
        }

        public void Save(GameData gameData)
        {
            string fullPath = Path.Combine(_dataPath, _dataName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? "");

                string dataToStore = JsonUtility.ToJson(gameData, true);

                using FileStream   stream = new(fullPath, FileMode.Create);
                using StreamWriter writer = new(stream);
                writer.Write(dataToStore);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving data to {fullPath}:\n{e}");
            }


            using (StreamWriter writer = new StreamWriter(_dataPath + _dataName))
            {
                string json = JsonUtility.ToJson(gameData, true);
                writer.Write(json);
            }
        }
    }
}
