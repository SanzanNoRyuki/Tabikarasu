using System;
using UnityEngine;
using UnityEngine.Events;
using Utility.Attributes;

namespace Attacks.Affectables
{
    /// <summary>
    /// Object capable of getting wet.
    /// </summary>
    [DisallowMultipleComponent]
    public class Splashable : MonoBehaviour
    {
        /// <summary>
        /// Is this object is currently wet?
        /// </summary>
        [field: Fixed]
        [field: SerializeField]
        public bool IsWet { get; private set; }
        
        [SerializeField]
        [Tooltip("Object got wet.")]
        private UnityEvent splashed;

        [SerializeField]
        [Tooltip("Object got dry.")]
        private UnityEvent dried;

        private bool _awoken;

        /// <summary>
        /// Object got wet.
        /// </summary>
        public event Action Splashed;
        
        /// <summary>
        /// Object got dry.
        /// </summary>
        public event Action Dried;

        private void Awake()
        {
            _awoken = true;
            
            if (IsWet)
            {
                Splash();
            }
            else
            {
                Dry();
            }
        }

        /// <summary>
        /// Make this object wet.
        /// </summary>
        [ContextMenu("Splash")]
        public bool Splash()
        {
            if (IsWet && !_awoken) return false;
            _awoken = false;
            
            if (TryGetComponent(out Flammable flammable)) flammable.Extinguish();

            IsWet = true;
            splashed.Invoke();
            Splashed?.Invoke();
            return true;
        }

        /// <summary>
        /// Dry this object.
        /// </summary>
        [ContextMenu("Dry")]
        public bool Dry()
        {
            if (!IsWet && !_awoken) return false;
            _awoken = false;
        
            IsWet = false;
            dried.Invoke();
            Dried?.Invoke();
            return true;
        }
    }
}