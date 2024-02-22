using System;
using Characters.Healths;
using UnityEngine;
using UnityEngine.Events;
using Utility.Attributes;

namespace Attacks.Affectables
{
    /// <summary>
    /// Object capable of catching fire.
    /// </summary>
    public class Flammable : MonoBehaviour
    {
        /// <summary>
        /// Fire damage per second.
        /// </summary>
        [field: SerializeField]
        [field: Range(Health.MinDamage, Health.MaxDamage)]
        [field: Tooltip("Damage to inflict on the target.")]
        public int Damage { get; private set; } = 1;
        
        /// <summary>
        /// Is this object is currently on fire?
        /// </summary>
        [field: Fixed]
        [field: SerializeField]
        public bool IsLit { get; private set; }

        private bool   _awaken;
        private Health _health;

        [SerializeField]
        [Tooltip("Object got lit.")]
        private UnityEvent lit;

        [SerializeField]
        [Tooltip("Object got extinguished.")]
        private UnityEvent extinguished;

        /// <summary>
        /// Object got lit.
        /// </summary>
        public event Action Lit;

        /// <summary>
        /// Object got extinguished.
        /// </summary>
        public event Action Extinguished;


        private void Awake()
        {
            _awaken = true;
            
            if (IsLit)
            {
                Light();
            }
            else
            {
                Extinguish();
            }
        }

        private void OnEnable()
        {
            if (_health == null) enabled = false;
        }

        private void Update()
        {
            if (_health.IsAlive)
            {
                _health.Damage(Damage);
            }
            else
            {
                Extinguish();
            }
        }

        /// <summary>
        /// Light this object on fire.
        /// </summary>
        [ContextMenu("Light")]
        public bool Light()
        {
            if (IsLit && !_awaken) return false;
            _awaken = false;

            if (TryGetComponent(out Splashable splashable)) splashable.Dry();
            
            if (TryGetComponent(out _health)) enabled = true;

            IsLit = true;
            lit.Invoke();
            Lit?.Invoke();
            return true;
        }

        /// <summary>
        /// Extinguish this object.
        /// </summary>
        [ContextMenu("Extinguish")]
        public bool Extinguish()
        {
            if (!IsLit && !_awaken) return false;
            _awaken = false;

            if (TryGetComponent(out _health)) enabled = false;

            IsLit = false;
            extinguished.Invoke();
            Extinguished?.Invoke();
            return true;
        }
    }
}