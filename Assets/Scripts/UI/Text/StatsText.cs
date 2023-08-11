using TMPro;
using UnityEngine;
using Utility.Save_System;
using Utility.Save_System.Data;

namespace UI.Text
{
    /// <summary>
    /// Displays stats at the end of the game.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class StatsText : MonoBehaviour, IPersistentDataHandler
    {
        [SerializeField]
        [Range(1, 100)]
        private int gemValue = 5;
        
        private int _coins;
        private int _gems;
        private int _deaths;

        private void Start()
        {
            GetComponent<TextMeshProUGUI>().text = $"<align=\"center\"><style=\"H2\">Your path was adorned with <color=#ffff40>{_coins + _gems * gemValue}</color> treasures reflecting your resourcefulness and tenacity.\n\nYet, as with any great odyssey, the road was not without its perils.\n\nYou encountered <color=#ffff40>{_deaths}</color> instances where fate did not align in your favor. Each setback, however, only served to strengthen your resolve, teaching valuable lessons and pushing you further towards your goal.</style></align>";
        }

        /// <summary>
        /// Does nothing here.
        /// </summary>
        /// <param name="gameData">Saved game data.</param>
        public void Save(ref GameData gameData)
        {
            // Do nothing.
        }
        /// <summary>
        /// Loads stats from the game data.
        /// </summary>
        /// <param name="gameData">Saved game data.</param>
        public void Load(GameData gameData)
        {
            _coins  = gameData.Coins;
            _gems   = gameData.Gems;
            _deaths = gameData.Deaths;
        }
    }
}