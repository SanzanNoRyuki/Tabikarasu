using Characters.Behaviour;
using Characters.Healths;
using UnityEngine;

namespace Video.Animators
{
    /// <summary>
    /// Orc animation state handler.
    /// </summary>
    /// <remarks>
    /// Heavily inspired by <see href="https://www.youtube.com/watch?v=ZwLekxsSY3Y">Tarodev</see>.
    /// </remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class OrcAnimator : StateAnimator
    {
        [SerializeField]
        private bool isShaman;

        private Rigidbody2D    _rigidbody;
        private Health         _health;

        private bool _respawned;
        private bool _hurt;

        /// <summary>
        /// Orc is attacking.
        /// </summary>
        [field: SerializeField]
        public bool Attacking { get; private set; }

        private void OnEnable()
        {
            _rigidbody = GetComponentInParent<Rigidbody2D>();
            _health = GetComponentInParent<Health>();

            if (_rigidbody == null || _health == null)
            {
                Debug.LogWarning($"Missing required components on {transform.parent.name} - {nameof(Rigidbody2D)}: {_rigidbody}, {nameof(Health)}: {_health}", this);
                enabled = false;
            }
            else
            {
                _health.Respawned += OnRespawn;
                _health.Damaged += OnDamage;
            }
        }
        private void OnDisable()
        {
            if (_health == null) return;
            
            _health.Respawned -= OnRespawn;
            _health.Damaged -= OnDamage;
        }

        private void OnDamage(int obj)
        {
            _hurt = true;
        }

        private void OnRespawn()
        {
            _respawned = true;
        }

        protected override int GetState()
        {
            if (_respawned)
            {
                ResetState();
                return Cleanup(Lock(States.Respawning));
            }

            if (_hurt)
            {
                ResetState();
                return Cleanup(Lock(States.Hurt));
            }

            if (Attacking)
            {
                ResetState();
                return Cleanup(Lock(States.Attacking));
            }

            return _rigidbody.velocity.x switch
                   {
                       > 10f  => States.Running,
                       > 0.1f => States.Walking,
                       _      => isShaman ? States.Casting : States.Idle
                   };
        }

        private int Cleanup(int state)
        {
            _respawned = false;
            _hurt = false;
            Attacking = false;
            return state;
        }


        /// <summary>
        /// Names for animator system variables.
        /// </summary>
        private static class States
        {
            public static readonly int Idle              = Animator.StringToHash("Idle");
            public static readonly int Walking           = Animator.StringToHash("Walk");
            public static readonly int Running           = Animator.StringToHash("Run");
            public static readonly int Attacking         = Animator.StringToHash("Attack");
            public static readonly int Hurt              = Animator.StringToHash("Hurt");
            public static readonly int Respawning        = Animator.StringToHash("Respawn");
            public static readonly int Dying             = Animator.StringToHash("Death");
            public static readonly int Casting           = Animator.StringToHash("Cast");
        }
    }
}