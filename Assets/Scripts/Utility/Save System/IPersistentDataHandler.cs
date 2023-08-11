using Utility.Save_System.Data;

namespace Utility.Save_System
{
    public interface IPersistentDataHandler
    {
        /// <summary>
        /// Modify stored game data.
        /// </summary>
        /// <param name="gameData">Saved game data.</param>
        public void Save(ref GameData gameData);

        /// <summary>
        /// Load stored game data.
        /// </summary>
        /// <param name="gameData">Saved game data.</param>
        public void Load(GameData gameData);
    }
}