using System;
using Characters.Player.Skills.Elemental.Passives;
using Environment.Decorations;
using Environment.Interactive.Doors;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utility.Attributes;

namespace Characters.Healths
{
    /// <summary>
    /// Object capable of taking damage.
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Health : ProgressCondition
    {
        /// <summary>
        /// Minimum damage (or healing) that can be inflicted in a single strike.
        /// </summary>
        public const int MinDamage = 0;

        /// <summary>
        /// Maximum damage (or healing) that can be inflicted in a single strike.
        /// </summary>
        public const int MaxDamage = 1000;

        /// <summary>
        /// Full health of the object.
        /// </summary>
        [field: SerializeField]
        [field: Range(1, 100000)]
        [field: Tooltip("Full health of the object.")]
        public int Full { get; private set; } = 1;

        /// <summary>
        /// Current health of the object.
        /// </summary>
        [field: SerializeField]
        [field: Range(1, 100000)]
        [field: Tooltip("Current health of the object.")]
        public int Current { get; private set; }

        [SerializeField]
        private Shield shield;

        [Fixed]
        [SerializeField]
        [Tooltip("Object spawned on death.")]
        private Corpse corpse;
        private Corpse _corpse;

        [SerializeField]
        private UnityEvent respawned;

        [SerializeField]
        private UnityEvent died;

        private Rigidbody2D _rb;

        /// <summary>
        /// Health got damaged by passed value.
        /// </summary>
        public event Action<int> Damaged;

        /// <summary>
        /// Health is dying. Invoked <b>before</b> this object is deactivated.
        /// </summary>
        public event Action Died;

        /// <summary>
        /// Health got revived.
        /// </summary>
        public event Action Respawned;

        

        public bool IsAlive => Current > 0;

        protected virtual void Awake()
        {
            Current = Full;
            shield = GetComponentInChildren<Shield>();
            _rb     = GetComponent<Rigidbody2D>();

            if (corpse != null)
            {
                _corpse = Instantiate(corpse);
                _corpse.gameObject.SetActive(false);
            }
        }

        protected virtual void OnValidate()
        {
            if (Application.isPlaying) return;

            Current = Full;
        }

        /// <summary>
        /// Inflict damage to this health.
        /// </summary>
        /// <param name="damage">Damage inflicted.</param>
        [ContextMenu("Damage this health by one.")]
        public void Damage(int damage = 1)
        {
            damage = Mathf.Clamp(damage, MinDamage, MaxDamage);

            if (shield != null && !shield.IsBroken)
            {
                shield.Break();
                return;
            }

            Current -= damage;
            Current = Mathf.Clamp(Current, 0, Full);

            if (Current <= 0)
            {
                Kill();
            }
            else
            {
                Damaged?.Invoke(damage);
            }
        }

        /// <summary>
        /// Kill this health.
        /// </summary>
        [ContextMenu("Kill")]
        public virtual void Kill()
        {
            // Can't kill what's already dead.
            //if (!IsAlive) return;
            Current = 0;

            died.Invoke();
            Died?.Invoke();

            gameObject.SetActive(false);

            if (_corpse != null)
            {
                Transform tf = transform;
                _corpse.Spawn(tf.position, tf.rotation);
            }
        }

        /// <summary>
        /// Revive/Restore this health.
        /// </summary>
        [ContextMenu("Respawn")]
        public virtual void Respawn()
        {
            if (IsAlive) return;

            _rb.velocity = Vector2.zero;
            
            Current = Full;

            gameObject.SetActive(true);
            respawned.Invoke();
            Respawned?.Invoke();

            if (_corpse != null) _corpse.gameObject.SetActive(false);
        }

        /// <summary>
        /// Is this object death?
        /// </summary>
        /// <returns>Returns true if this object is considered dead. False otherwise.</returns>
        public bool IsDead()
        {
            return Current <= 0;
        }

        /// <summary>
        /// <inheritdoc cref="ProgressCondition.ConditionMet"/>
        /// </summary>
        /// <returns>Returns true if health is dead.</returns>
        public override bool ConditionMet()
        {
            return IsDead();
        }
    }
}