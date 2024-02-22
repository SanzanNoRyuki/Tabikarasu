using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Attributes;

namespace Attacks.Volleys
{
    /// <summary>
    /// <see cref="Volley">Volley pattern</see> sequence.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Volley Sequence", menuName = "ScriptableObjects/Volley Sequence")]
    public class VolleySequence : ScriptableObject
    {
        /// <summary>
        /// Time between volley patterns.
        /// </summary>
        [field: SerializeField]
        [field: FixedRange(0f, 5f)]
        [field: Tooltip("Time between volley patterns.")]
        public float TimeBetweenVolleys { get; private set; } = 0.1f;

        [SerializeField]
        [Tooltip("Volley patterns.")]
        private Volley[] volleys;

        /// <summary>
        /// Volley patterns.
        /// </summary>
        public IEnumerable<Volley> Volleys => volleys ?? Enumerable.Empty<Volley>();
    }
}