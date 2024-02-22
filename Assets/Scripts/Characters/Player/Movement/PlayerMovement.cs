using System;
using System.Collections;
using Input_System.Game_Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility.Attributes;
using Utility.Constants;
using Utility.Extensions;
using Utility.Physics_Checks;

namespace Characters.Player.Movement
{
    /// <summary>
    /// Player movement module.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CoyoteGroundCheck))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(WallCheck))]
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// Minimum possible number of extra jumps.
        /// </summary>
        public const int MinExtraJumps = 0;

        /// <summary>
        /// Maximum possible number of extra jumps.
        /// </summary>
        public const int MaxExtraJumps = 1;

        [Header("Run")]
        
        [ReadOnly]
        [SerializeField]
        [Tooltip("Player external input.")]
        private Vector2 playerMoveInput;
        
        [SerializeField]
        [Tooltip("Virtual input used by other scripts.")]
        private Vector2 virtualMoveInput;

        [field: ReadOnly]
        [field: SerializeField]
        [field: Tooltip("Modifier used by other scripts.")]
        public float RunSpeedModifier { get; set; } = 1f;

        [SerializeField]
        [Range(0f, 100f)]
        private float runSpeed = 14f;

        [SerializeField]
        [Range(0.1f, 10f)]
        private float accelerationRate = 4f;

        [SerializeField]
        [Range(0.1f, 10f)]
        private float decelerationRate = 8f;

        [SerializeField]
        [Range(0.01f, 1f)]
        [Tooltip("Raises speed to set power to make it feel more responsive.")]
        private float speedPower = 0.87f;

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Friction applied to the player. Used instead of material to prevent wall sticking.")]
        private float friction = 0.5f;

        [field: Header("Climb")]

        [field: ReadOnly]
        [field: SerializeField]
        public bool IsClimbing { get; private set; }

        [SerializeField]
        private LayerMask climbable;

        [SerializeField]
        [Range(0f, 100f)]
        private float climbSpeed = 7f;

        [SerializeField]
        [Range(BufferTimeConstants.Min, BufferTimeConstants.Max)]
        [Tooltip(BufferTimeConstants.Tooltip)]
        private float descendBufferTime = 0.1f;
        private float _descendBufferTimeCounter;

        [Header("Jump")]

        [ReadOnly]
        [SerializeField]
        [Tooltip("Is currently mid-jump?")]
        private bool isJumping;

        [SerializeField]
        [Range(0f, 32f)]
        [Tooltip("Jump height in blocks.")]
        private float jumpHeight = 4f;

        [ReadOnly]
        [SerializeField]
        [Tooltip("Force used to jump.")]
        private float jumpForce;
        
        [SerializeField]
        [Range(0.1f, 10f)]
        [Tooltip("Cooldown between normal subsequent jumps.")]
        private float jumpCooldown = 0.1f;
        private float _lastJumpTime;

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Multiplier applied to the jump force on jump button release.")]
        private float jumpReleaseMultiplier = 0.5f;

        [SerializeField]
        [Range(BufferTimeConstants.Min, BufferTimeConstants.Max)]
        [Tooltip(BufferTimeConstants.Tooltip)]
        private float jumpBufferTime = 0.2f;
        private float _jumpBufferTimeCounter;

        [SerializeField]
        [Range(MinExtraJumps, MaxExtraJumps)]
        [Tooltip("Possible mid-air jumps.")]
        private int extraJumps;
        
        [ReadOnly]
        [SerializeField]
        [Tooltip("Available extra jumps.")]
        private int extraJumpTokens;

        [Header("Dash")]

        [ReadOnly]
        [SerializeField]
        private bool isDashing;

        [ReadOnly]
        [SerializeField]
        private float dashSpeed;

        [SerializeField]
        [Range(0.1f, 10f)]
        private float dashLength = 5f;

        [SerializeField]
        [Range(0.01f, 5f)]
        private float dashDuration = 0.5f;
        private WaitForSeconds _dashDuration;

        //[SerializeField]
        //[Range(0f, 10f)]
        //private float dashCooldown = 2f;

        [SerializeField]
        [Range(BufferTimeConstants.Min, BufferTimeConstants.Max)]
        [Tooltip(BufferTimeConstants.Tooltip)]
        private float dashBufferTime = 0.2f;
        private float _dashBufferTimeCounter;

        [Header("Other")]
        
        [SerializeField]
        [Range(0f, 100f)]
        [Tooltip("Maximum possible fall speed.")]
        private float fallSpeedLimit = 40f;

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Gravity multiplier applied to the player by default.")]
        private float normalGravityMultiplier = 1f;

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Gravity multiplier applied to the player when climbing.")]
        private float climbGravityMultiplier;

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Gravity multiplier applied to the player when falling.")]
        private float fallGravityMultiplier = 2f;

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Gravity multiplier applied to the player when dashing.")]
        private float dashGravityMultiplier = 0f;

        // Private fields:
        private Rigidbody2D       _rb;
        private CoyoteGroundCheck _groundCheck;
        private WallCheck         _wallCheck;
        private Descendable       _descendable;
        private Coroutine         _dashing;
        

        #region Events

        [SerializeField]
        private UnityEvent onFlip;
        public event Action OnFlip;

        [SerializeField]
        private UnityEvent onJump;
        public event Action Jumped;

        [SerializeField]
        private UnityEvent onAirJump;
        public event Action OnAirJump;

        [SerializeField]
        private UnityEvent onJumpStop;
        public event Action OnJumpStop;
        [SerializeField]
        private UnityEvent onDash;
        public event Action OnDash;
        [SerializeField]
        private UnityEvent onDashStop;
        public event Action OnDashStop;

        [SerializeField]
        private UnityEvent onGroundJump;
        public event Action OnGroundJump;

        [SerializeField]
        private UnityEvent onPhaseDash;
        public event Action OnPhaseDash;


        


        #endregion

        /// <summary>
        /// Input determining player movement. Virtual input has priority over player's. Input has to be in &lt;-1,1&gt; range.
        /// </summary>
        public Vector2 MoveInput
        {
            // Virtual input has priority.
            get => virtualMoveInput != Vector2.zero ? virtualMoveInput : playerMoveInput;

            set => virtualMoveInput = Vector2.ClampMagnitude(value, 1f);
        }

        private void Awake()
        {
            _rb          = GetComponent<Rigidbody2D>();
            _groundCheck = GetComponent<CoyoteGroundCheck>();
            _wallCheck   = GetComponent<WallCheck>();

            ResetProperties();
        }

        private void Update()
        {
            ProcessMoveInput();
            ProcessJumpInput();
            ProcessDashInput();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying) return;

            ResetProperties();
        }

        private void OnEnable()
        {
            GameController.Subscribe(GameController.Actions.Move, ParseMoveInput);
            GameController.Subscribe(GameController.Actions.Jump, ParseJumpInput);
            GameController.Subscribe(GameController.Actions.Dash, ParseDashInput);

            //_groundCheck.Landed  += ResetJump;
            _wallCheck.GotWalled += InterruptDash;

            
        }

        private void OnDisable()
        {
            GameController.Unsubscribe(GameController.Actions.Move, ParseDashInput);
            GameController.Unsubscribe(GameController.Actions.Jump, ParseJumpInput);
            GameController.Unsubscribe(GameController.Actions.Dash, ParseDashInput);

            //_groundCheck.Landed  -= ResetJump;
            _wallCheck.GotWalled -= InterruptDash;
        }


        public void ResetJumpTokens()
        {
            isJumping = false;
            extraJumpTokens = 1 + extraJumps;
        }

        

        private bool CanGroundJump => _groundCheck.Grounded && Time.time - _lastJumpTime >= jumpCooldown; 

        /// <summary>
        /// Can the player jump?
        /// </summary>
        /// <returns>Returns true if player accepts jump input. False otherwise.</returns>
        public bool CanJump()
        {
            return (!isDashing && !isJumping && !IsPulling && _groundCheck.CoyoteGrounded) || CanExtraJump();
        }


        public void Jump(Vector2 direction, float multiplier = 1f)
        {
            InterruptClimb();
            InterruptDash();

            _rb.AddForce(direction * (jumpForce * multiplier), ForceMode2D.Impulse);

            onJump?.Invoke();
            Jumped?.Invoke();

            
            if (_groundCheck.CoyoteGrounded && Time.time - _lastJumpTime >= jumpCooldown)
            {
                onGroundJump.Invoke();
                OnGroundJump?.Invoke();
            }
            else
            {
                onAirJump.Invoke();
                OnAirJump?.Invoke();
            }
            

            isJumping = true;

            extraJumpTokens = Mathf.Max(extraJumpTokens - 1, 0);

            _lastJumpTime = Time.time;
        }


        private void FixedUpdate()
        {
            if (IsClimbing)
            {
                Climb(playerMoveInput);
            }
            else if (CanMove())
            {
                MoveHorizontally(playerMoveInput.x);
            }

            ApplyFriction();
            ApplyGravity();
            LimitFallSpeed();
        }

        
        
        // //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////









        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent<Descendable>(out var descendable)) _descendable = descendable;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out Descendable descendable) && descendable == _descendable) _descendable = null;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsClimbing && !CanClimb()) InterruptClimb();
        }



















        
        #region Main

        

        /*
         * Make this always active, fix land event to be called sometimes if stuck.
         */

        

        

        

        

        #endregion

        #region Input Parsing

        private void ParseMoveInput(InputAction.CallbackContext obj)
        {
            switch (obj.ReadValue<Vector2>().y)
            {
                case < 0f:
                    _descendBufferTimeCounter = descendBufferTime;

                    IsClimbing = CanClimb();
                    break;
                case > 0f:
                    IsClimbing = CanClimb();
                    break;
            }

            if (IsClimbing) InterruptDash();
        }

        private void ParseJumpInput(InputAction.CallbackContext obj)
        {
            if (obj.ReadValueAsButton())
            {
                _jumpBufferTimeCounter = jumpBufferTime;
            }
            else
            {
                if (_rb.velocity.y > 0f)
                {
                    _rb.velocity *= new Vector2(1f, jumpReleaseMultiplier);
                }
            }
        }

        private void ParseDashInput(InputAction.CallbackContext obj)
        {
            _dashBufferTimeCounter = dashBufferTime;
        }

        #endregion

        #region Input Processing

        private void ProcessMoveInput()
        {
            playerMoveInput = GameController.MoveInput.Value;


            if (!isDashing)
            {
                switch (playerMoveInput.x)
                {
                    case < 0f when transform.FacingRight2D():
                        transform.FlipX();
                        onFlip.Invoke();
                        OnFlip?.Invoke();
                        break;
                    case > 0f when transform.FacingLeft2D():
                        transform.FlipX();
                        onFlip.Invoke();
                        OnFlip?.Invoke();
                        break;
                }
            }


            if (_descendBufferTimeCounter > 0f)
            {
                if (CanDescend())
                {
                    Descend();
                    _descendBufferTimeCounter = 0f;
                }
                else
                {
                    _descendBufferTimeCounter -= Time.deltaTime;
                }
            }
        }
        private void ProcessJumpInput()
        {
            if (isJumping && _groundCheck.Grounded && Time.time - _lastJumpTime >= jumpCooldown) ResetJumpTokens();
            
            if (_jumpBufferTimeCounter > 0f)
            {
                if (CanJump())
                {
                    Jump();
                    _jumpBufferTimeCounter = 0f;
                }
                else
                {
                    _jumpBufferTimeCounter -= Time.deltaTime;
                }
                
            }
        }

        private void ProcessDashInput()
        {
            if (_dashBufferTimeCounter > 0f)
            {
                if (CanDash())
                {
                    if (IsPhasing)
                    {
                        PhaseDash();
                    }
                    else
                    {
                        Dash();
                    }
                    _dashBufferTimeCounter = 0f;
                }
                else
                {
                    _dashBufferTimeCounter -= Time.deltaTime;
                }
            }
        }

        public bool IsPulling { get; set; }

        #endregion

        #region Permission Flags

        /// <summary>
        /// Can the player move?
        /// </summary>
        /// <returns>Returns true if player accepts move input. False otherwise.</returns>
        public bool CanMove()
        {
            return !isDashing && !IsClimbing;
        }

        private readonly RaycastHit2D[] _results = new RaycastHit2D[1];

        /// <summary>
        /// Can the player climb?
        /// </summary>
        /// <returns>Returns true if player accepts climb input. False otherwise.</returns>
        public bool CanClimb()
        {
            return Physics2D.Raycast(transform.position, Vector2.up, 0.1f, climbable).collider != null;
        }

        

        /// <summary>
        /// Can the player preform mid-air jump?
        /// </summary>
        /// <returns>Returns true if player accepts jump input in mid-air. False otherwise.</returns>
        public bool CanExtraJump()
        {
            return !isDashing && !IsPulling && extraJumpTokens > 0;
        }

        /// <summary>
        /// Can the player dash?
        /// </summary>
        /// <returns>Returns true if player accepts dash input. False otherwise.</returns>
        public bool CanDash()
        {
            return !isDashing;
        }

        #endregion









        // Reset jump on interact

        private void TriggerJumpStop()
        {
            onJumpStop?.Invoke();
            OnJumpStop?.Invoke();
        }

        private void TriggerDash()
        {
            if (IsPhasing)
            {
                onPhaseDash.Invoke();
                OnPhaseDash?.Invoke();
            }
            else
            {
                onDash.Invoke();
                OnDash?.Invoke();
            }
        }

        private void TriggerDashStop()
        {
            onDashStop?.Invoke();
            OnDashStop?.Invoke();
        }



        
        public void MoveHorizontally(float direction)
        {
            InterruptClimb();
            InterruptDash();

            var speedDifference = Mathf.Clamp(direction, -1f, 1f) * runSpeed * RunSpeedModifier - _rb.velocity.x;
            var acceleration    = Mathf.Approximately(direction, 0f) ? decelerationRate : accelerationRate;
            var force           = Mathf.Sign(speedDifference) * _rb.mass * Mathf.Pow(Mathf.Abs(speedDifference) * acceleration, speedPower);

            _rb.AddForce(Vector2.right * force);

            if (Mathf.Approximately(direction, 0f)) ApplyFriction();
        }

        private void ApplyFriction()
        {
            if (IsClimbing) return;
            if (isDashing) return;
            if (!_groundCheck.Grounded) return;
            if (_rb.velocity.x == 0) return;

            var force = Mathf.Sign(_rb.velocity.x) * Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(friction));

            _rb.AddForce(Vector2.left * force, ForceMode2D.Impulse);
        }

        public void Climb(Vector2 direction)
        {
            _rb.MovePosition(_rb.position + direction * (climbSpeed * Time.fixedDeltaTime));
        }

        public void Jump(float multiplier = 1f)
        {
            Jump(Vector2.up, multiplier);
        }


        

        public void Dash()
        {
            InterruptClimb();
            InterruptDash();

            _dashing = StartCoroutine(Dashing());

            // If we're already walled, interrupt dash immediately.
            if (_wallCheck.Walled) InterruptDash();
        }

        private IEnumerator Dashing()
        {
            isDashing    = true;
            _rb.velocity = transform.right * dashSpeed;
            TriggerDash();

            yield return _dashDuration;

            InterruptDash();
        }

        public void PhaseDash()
        {
            Phase();
            Dash();
        }
        
        
        /// <summary>
        /// Phase dash is active.
        /// </summary>

        public bool IsPhasing { get; set; }

        public void Phase()
        {
            Physics2D.IgnoreLayerCollision(Layers.Player, Layers.Enemies, true);
            
        }

        public void PhaseReset()
        {
            Physics2D.IgnoreLayerCollision(Layers.Player, Layers.Enemies, false);
        }







        
        public void InterruptClimb()
        {
            if (!IsClimbing) return;

            IsClimbing = false;
        }

        public void InterruptDash()
        {
            if (!isDashing) return;

            if (_dashing != null) StopCoroutine(_dashing);

            _rb.gravityScale           = normalGravityMultiplier;
            _rb.velocity               = Vector2.zero;
            isDashing                  = false;
            TriggerDashStop();

            PhaseReset();
        }

        public void AddExtraJumps(int amount = 1)
        {
            extraJumps      =  Mathf.Clamp(extraJumps + amount, MinExtraJumps, MaxExtraJumps);
            extraJumpTokens += extraJumps;
        }

        public void RemoveExtraJumps(int amount = 1)
        {
            AddExtraJumps(-amount);
        }

        public void AllowPhaseDash()
        {
            IsPhasing = true;
        }

        public void DisallowPhaseDash()
        {
            IsPhasing = false;
        }














        private bool CanDescend()
        {
            return !isDashing && _descendable != null;
        }

        private void Descend()
        {
            _descendable.Descend(_rb);
        }

        

        private void UpdateJumpForce()
        {
            if (_rb == null) return;
            
            var gravity = normalGravityMultiplier * Physics2D.gravity.y;

            jumpForce = _rb.mass * Mathf.Sqrt(-2f * gravity * jumpHeight);
        }

        private void UpdateDashSpeed()
        {
            dashSpeed = dashLength / dashDuration;
        }

        private void ApplyGravity()
        {
            if (IsClimbing)
            {
                _rb.gravityScale = climbGravityMultiplier;
            }
            else if (isDashing)
            {
                _rb.gravityScale = dashGravityMultiplier;
            }
            else if (_rb.velocity.y < 0f)
            {
                _rb.gravityScale = fallGravityMultiplier;
            }
            else
            {
                _rb.gravityScale = normalGravityMultiplier;
            }
        }

        private void LimitFallSpeed()
        {
            if (_rb.velocity.y < -fallSpeedLimit)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -fallSpeedLimit);
            }
        }

        private void ResetProperties()
        {
            UpdateJumpForce();
            UpdateDashSpeed();
            _dashDuration = new WaitForSeconds(dashDuration);
        }
    }
}