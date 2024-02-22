using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Attributes;

namespace Attacks.Volleys
{
    /// <summary>
    /// Volley pattern.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Volley", menuName = "ScriptableObjects/Volley Pattern")]
    public class Volley : ScriptableObject
    {
        /// <summary>
        /// Every pattern wave is this long.
        /// </summary>
        public const int WaveLength = 9;
        
        /// <summary>
        /// Glyph used to indicate an empty space.
        /// </summary>
        public const char EmptyGlyph = '#';

        /// <summary>
        /// Glyph used to indicate a shot.
        /// </summary>
        [field: ReadOnly]
        [field: SerializeField]
        [field: Tooltip("Glyph used to indicate a shot.")]
        public char ShootGlyph { get; private set; } = '$';

        [field: Range(0.5f, 5f)]
        [field: SerializeField]
        public float TimeBetweenWaves { get; private set; } = 0.1f;
        public IEnumerable<string> Pattern => pattern ?? Enumerable.Empty<string>();

        [SerializeField]
        private List<string> pattern = new()
        {
            $"{new string(EmptyGlyph, WaveLength)}",
            $"{new string(EmptyGlyph, WaveLength)}",
            $"{new string(EmptyGlyph, WaveLength)}",
            $"{new string(EmptyGlyph, WaveLength)}",
            $"{new string(EmptyGlyph, WaveLength)}",
            $"{new string(EmptyGlyph, WaveLength)}",
            $"{new string(EmptyGlyph, WaveLength)}",
            $"{new string(EmptyGlyph, WaveLength)}",
            $"{new string(EmptyGlyph, WaveLength)}",
        };


        private void Awake()
        {
            FixPattern();
        }
        private void OnValidate()
        {
            FixPattern();
        }
        public void FixPattern()
        {
            pattern = pattern.Select(FixWave).ToList();
        }

        private string FixWave(string wave)
        {
            return new string(wave.Select(c => c == ShootGlyph ? ShootGlyph : EmptyGlyph).ToArray()).PadRight(WaveLength, EmptyGlyph)[..WaveLength];
        }




    }
}