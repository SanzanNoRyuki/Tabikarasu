using System;
using UnityEngine;
using Utility.Attributes;

namespace Characters.Behaviour
{
    /// <summary>
    /// Entity state.
    /// </summary>
    [Serializable]
    public class State
    {
        [field: SerializeField]
        [field: Tooltip("State identification.")]
        public StateType Type { get; private set; } = StateType.Idle;

        [field: SerializeField]
        [field: Tooltip("State associated behaviour.")]
        public EntityBehaviour Behaviour { get; private set; }

        [field: SerializeField]
        [field: Range(0.01f, 100f)]
        [field: Tooltip("Activation range for this state.")]
        public float Range { get; private set; }

        /// <summary>
        /// Possible entity states.
        /// </summary>
        public enum StateType
        {
            Idle,
            Chasing,
            Attacking,
        }
    }
}