using System;
using Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Attributes;

namespace Utility.Save_System.Data
{
    /// <summary>
    /// Persistent game data.
    /// </summary>
    [Serializable]
    public sealed class GameData
    {
        /// <summary>
        /// Minimum possible level.
        /// </summary>
        public const int MinLevel = 1;

        /// <summary>
        /// Minimum possible collected tokens count.
        /// </summary>
        public const int MinTokens = 0;

        /// <summary>
        /// Maximum possible collected tokens count.
        /// </summary>
        public const int MaxTokens = 1_000_000;

        /// <summary>
        /// Minimum possible player death count.
        /// </summary>
        public const int MinDeaths = 0;

        /// <summary>
        /// Maximum possible player death count.
        /// </summary>
        public const int MaxDeaths = 1_000_000;

        /// <summary>
        /// Default element.
        /// </summary>
        public const Element DefaultElement = Element.Air;

        /// <summary>
        /// Current level.
        /// </summary>
        /// <remarks>
        /// Use <see cref="Level"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Current level.")]
        [Obsolete("Use Level instead. Must be exposed for serialization.", false)]
        public int level = MinLevel;

        /// <summary>
        /// Collectible coins count.
        /// </summary>
        /// <remarks>
        /// Use <see cref="Coins"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Collectible coins count.")]
        [Obsolete("Use Coins instead. Must be exposed for serialization.", false)]
        public int coins = MinTokens;

        /// <summary>
        /// Collectibles gem count.
        /// </summary>
        /// <remarks>
        /// Use <see cref="Gems"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Collectibles gem count.")]
        [Obsolete("Use Gems instead. Must be exposed for serialization.", false)]
        public int gems = MinTokens;

        /// <summary>
        /// Player death count.
        /// </summary>
        /// <remarks>
        /// Use <see cref="Deaths"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Player death count.")]
        [Obsolete("Use Deaths instead. Must be exposed for serialization.", false)]
        public int deaths = MinDeaths;

        /// <summary>
        /// Selected element.
        /// </summary>
        /// <remarks>
        /// Use <see cref="Element"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Selected element.")]
        [Obsolete("Use Element instead. Must be exposed for serialization.", false)]
        public Element element = DefaultElement;

        /// <summary>
        /// Player unlocked air passive.
        /// </summary>
        /// <remarks>
        /// Use <see cref="AirPassive"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Player unlocked air passive.")]
        [Obsolete("Use AirPassive instead. Must be exposed for serialization.", false)]
        public bool airPassive;

        /// <summary>
        /// Player unlocked water passive.
        /// </summary>
        /// <remarks>
        /// Use <see cref="WaterPassive"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Player unlocked water passive.")]
        [Obsolete("Use WaterPassive instead. Must be exposed for serialization.", false)]
        public bool waterPassive;

        /// <summary>
        /// Player unlocked earth passive.
        /// </summary>
        /// <remarks>
        /// Use <see cref="EarthPassive"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Player unlocked earth passive.")]
        [Obsolete("Use EarthPassive instead. Must be exposed for serialization.", false)]
        public bool earthPassive;

        /// <summary>
        /// Player unlocked fire passive.
        /// </summary>
        /// <remarks>
        /// Use <see cref="FirePassive"/> instead. Must be exposed for serialization.
        /// </remarks>
        [ReadOnly]
        [Tooltip("Player unlocked fire passive.")]
        [Obsolete("Use FirePassive instead. Must be exposed for serialization.", false)]
        public bool firePassive;

        // Uses internal field marked as obsolete to discourage direct access.
        #pragma warning disable CS0618

        /// <summary>
        /// <inheritdoc cref="level"/>
        /// </summary>
        public int Level
        {
            get => level;

            set => level = Mathf.Clamp(value, MinLevel, SceneManager.sceneCountInBuildSettings - 1);
        }

        /// <summary>
        /// <inheritdoc cref="coins"/>
        /// </summary>
        public int Coins
        {
            get => coins;

            set => coins = Mathf.Clamp(value, MinTokens, MaxTokens);
        }

        /// <summary>
        /// <inheritdoc cref="gems"/>
        /// </summary>
        public int Gems
        {
            get => gems;

            set => gems = Mathf.Clamp(value, MinTokens, MaxTokens);
        }

        /// <summary>
        /// <inheritdoc cref="deaths"/>
        /// </summary>
        public int Deaths
        {
            get => deaths;

            set => deaths = Mathf.Clamp(value, MinDeaths, MaxDeaths);
        }

        /// <summary>
        /// <inheritdoc cref="element"/>
        /// </summary>
        public Element Element
        {
            get => element;

            set => element = Enum.IsDefined(typeof(Element), value) ? value : DefaultElement;
        }

        /// <summary>
        /// <inheritdoc cref="airPassive"/>
        /// </summary>
        public bool AirPassive
        {
            get => airPassive;

            set => airPassive = value;
        }

        /// <summary>
        /// <inheritdoc cref="waterPassive"/>
        /// </summary>
        public bool WaterPassive
        {
            get => waterPassive;

            set => waterPassive = value;
        }

        /// <summary>
        /// <inheritdoc cref="earthPassive"/>
        /// </summary>
        public bool EarthPassive
        {
            get => earthPassive;

            set => earthPassive = value;
        }

        /// <summary>
        /// <inheritdoc cref="firePassive"/>
        /// </summary>
        public bool FirePassive
        {
            get => firePassive;

            set => firePassive = value;
        }
        #pragma warning restore CS0618

        /// <summary>
        /// Set game data values to specified range.
        /// </summary>
        public GameData Sanitize()
        {
            Level        = Level;
            Coins        = Coins;
            Gems         = Gems;
            Deaths       = Deaths;
            Element      = Element;
            AirPassive   = AirPassive;
            WaterPassive = WaterPassive;
            EarthPassive = EarthPassive;
            FirePassive  = FirePassive;

            return this;
        }
    }
}