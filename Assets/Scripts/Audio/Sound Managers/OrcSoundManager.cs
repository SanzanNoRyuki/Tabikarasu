using System;
using Audio.Sound_Managers;
using Characters.Behaviours;
using Characters.Player.Movement;
using UnityEngine;
using Utility.Attributes;
using Utility.Physics_Checks;

namespace Audio.CharacterSoundManagers
{
    /// <summary>
    /// Player sound manager.
    /// </summary>
    public class OrcSoundManager : LocalSoundManager
    {
        [ReadOnly]
        [SerializeField]
        private State state = State.Idle;

        [Header("Sounds")]

        [SerializeField]
        private Sound idle;

        [SerializeField]
        private Sound walking;

        [SerializeField]
        private Sound snowWalking;
        
        [SerializeField]
        private Sound boxWalking;

        [SerializeField]
        private Sound climbing;
        
        [SerializeField]
        private Sound casting;

        [Header("Ground materials")]

        [SerializeField]
        private PhysicsMaterial2D snow;

        [SerializeField]
        private PhysicsMaterial2D wood;

        private PlayerMovement _movement;
        private PlayerSpell    _playerSpell;
        private GroundCheck    _groundCheck;
        private Rigidbody2D    _rigidbody2D;

        private enum State
        {
            Idle,
            Walking,
            SnowWalking,
            WoodWalking,
            Climbing,
            Casting,
        }

        private void OnEnable()
        {
            _playerSpell = GetComponentInParent<PlayerSpell>();
            _movement    = GetComponentInParent<PlayerMovement>();
            _groundCheck = GetComponentInParent<GroundCheck>();
            _rigidbody2D = GetComponentInParent<Rigidbody2D>();


            if (_playerSpell == null || _movement == null || _groundCheck == null || _rigidbody2D == null)
            {
                Debug.LogWarning($"Missing required components on {transform.parent.name} - {nameof(PlayerSpell)}: {_playerSpell}, {nameof(PlayerMovement)}: {_movement}, {nameof(GroundCheck)}: {_groundCheck}, {nameof(Rigidbody2D)}: {_rigidbody2D}", this);
                enabled = false;
            }
        }
        
        private void Update()
        {
            State newState = GetState();
            if (newState == state) return;

            StopLoop();
            state = newState;
            
            switch (state)
            {
                case State.Idle:
                    StartLoop(idle);
                    break;
                case State.Walking:
                    StartLoop(walking);
                    break;
                case State.SnowWalking:
                    StartLoop(snowWalking);
                    break;
                case State.WoodWalking:
                    StartLoop(boxWalking);
                    break;
                case State.Climbing:
                    StartLoop(climbing);
                    break;
                case State.Casting:
                    StartLoop(casting);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private State GetState()
        {
            if (_playerSpell.IsCasting) return State.Casting;
            if (_movement.IsClimbing)   return _movement.MoveInput.sqrMagnitude == 0 ? State.Idle : State.Climbing;
            if (_groundCheck.Grounded) return _movement.MoveInput.x             == 0 ? State.Idle : GetWalkingState();
            return State.Idle;
        }

        private State GetWalkingState()
        {
            if (_groundCheck.GroundCollider == null) return State.Walking;
            if (!_groundCheck.GroundCollider.TryGetComponent(out Collider2D otherCollider)) return State.Walking;
            
            PhysicsMaterial2D material = otherCollider.sharedMaterial;

            // ReSharper disable ConvertIfStatementToReturnStatement
            if (material == null) return State.Walking;
            if (material == wood) return State.WoodWalking;
            if (material == snow) return State.SnowWalking;
            // ReSharper restore ConvertIfStatementToReturnStatement
            
            return State.Walking;
        }
    }


}


// Triggers:
// Dashed / PhaseDashed
// Jumped / ExtraJumped
// Landed / SnowLanded / BoxLanded

// Loops:
// PlayerCast
// Climbing
// Walking / SnowWalking Based on level