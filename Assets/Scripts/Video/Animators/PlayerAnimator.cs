using Characters.Behaviour;
using Characters.Player.Movement;
using UnityEngine;
using Utility.Physics_Checks;

namespace Video.Animators
{
    /// <summary>
    /// Player animation state handler.
    /// </summary>
    /// <remarks>
    /// Heavily inspired by <see href="https://www.youtube.com/watch?v=ZwLekxsSY3Y">Tarodev</see>.
    /// </remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class PlayerAnimator : StateAnimator
    {
        /// <summary>
        /// Player jumped.
        /// </summary>
        [field: SerializeField]
        public bool Jumped { get; private set; }

        /// <summary>
        /// Player is dashing.
        /// </summary>
        [field: SerializeField]
        public bool Dashing { get; set; }

        /// <summary>
        /// Phase dash is active.
        /// </summary>
        [field: SerializeField]
        public bool IsPhasing { get; set; }
        
        private Rigidbody2D    _rigidbody;
        private GroundCheck    _groundCheck;
        private PlayerMovement _playerMovement;
        
        private bool           _grounded;
        private float          _xInput;
        private float          _ySpeed;
        
        private bool InDashingState => CurrentState == States.Dashing || CurrentState == States.JumpDashing || CurrentState == States.PhaseDashing || CurrentState == States.PhaseJumpDashing;
        private bool InJumpingState => CurrentState == States.Jumping;


        private void OnEnable()
        {
            _rigidbody      = GetComponentInParent<Rigidbody2D>();
            _groundCheck    = GetComponentInParent<GroundCheck>();
            _playerMovement = GetComponentInParent<PlayerMovement>();

            if (_rigidbody == null || _groundCheck == null || _playerMovement == null)
            {
                Debug.LogWarning($"Missing required components on {transform.parent.name} - {nameof(Rigidbody2D)}: {_rigidbody}, {nameof(GroundCheck)}: {_groundCheck}, {nameof(PlayerMovement)}: {_playerMovement}", this);
                enabled = false;
            }
        }

        protected override void Update()
        {
            _grounded = _groundCheck.Grounded;
            _xInput   = _playerMovement.MoveInput.x;
            _ySpeed   = _rigidbody.velocity.y;

            base.Update();
        }

        // Get the current animation state.
        protected override int GetState()
        {
            // Player is dashing.
            if (Dashing)
            {
                if (_playerMovement.IsPhasing)
                {
                    return InDashingState ? CurrentState : Cleanup(_grounded ? Lock(States.PhaseDashing) : Lock(States.PhaseJumpDashing));
                }
                else
                {
                    return InDashingState ? CurrentState : Cleanup(_grounded ? Lock(States.Dashing) : Lock(States.JumpDashing));
                }
            }

            // Player finished dash.
            // if (InDashingState) return Cleanup(Lock(AnimatorParameters.DashStopping));

            // Player jumped.
            if (Jumped)
            {
                if (InJumpingState) Replay();
                return Cleanup(Lock(States.Jumping));
            }

            // Player is on the ground.
            if (_grounded)
            {
                return _xInput != 0 ? States.Running : States.Idle;
            }

            // Player is in the air.
            return _ySpeed switch
                   {
                       > 0 => States.Jumping,
                       < 0 => States.Falling,
                       _   => CurrentState
                   };
        }

        private int Cleanup(int state)
        {
            Jumped = false;

            return state;
        }

        /// <summary>
        /// Names for animator system variables.
        /// </summary>
        private static class States
        {
            public static readonly int Idle              = Animator.StringToHash("Idle");
            public static readonly int Running           = Animator.StringToHash("Running");
            public static readonly int Jumping           = Animator.StringToHash("Jumping");
            public static readonly int Falling           = Animator.StringToHash("Falling");
            public static readonly int Landing           = Animator.StringToHash("Landing");
            public static readonly int Dashing           = Animator.StringToHash("Dashing");
            public static readonly int JumpDashing       = Animator.StringToHash("JumpDashing");
            public static readonly int DashStopping      = Animator.StringToHash("DashStopping");
            public static readonly int PhaseDashing      = Animator.StringToHash("PhaseDashing");
            public static readonly int PhaseJumpDashing  = Animator.StringToHash("PhaseJumpDashing");
            public static readonly int PhaseDashStopping = Animator.StringToHash("PhaseDashStopping");
        }
    }

    
}