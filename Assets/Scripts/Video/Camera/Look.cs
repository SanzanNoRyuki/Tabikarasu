using Cinemachine;
using Input_System.Game_Controller;
using UnityEngine;
using Utility.Extensions;

namespace Video.Camera
{
    /// <summary>
    /// Camera movement module allowing looking around.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CinemachineCameraOffset))]
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class Look : MonoBehaviour
    {
        [Range(1f, 10f)]
        [SerializeField]
        [Tooltip("Speed of camera movement.")]
        private float speed = 3f;

        [Range(1f, 10f)]
        [SerializeField]
        [Tooltip("Reset speed multiplier.")]
        private float resetMultiplier = 2f;

        [Range(0f, 10f)]
        [SerializeField]
        [Tooltip("Offset limit.")]
        private float radius = 7.5f;

        private CinemachineCameraOffset _cameraOffset;

        private Rigidbody2D _rb;

        private Vector2 _destination;

        private float   _multiplier;
        private Vector2 _input;

        protected void Awake()
        {
            _cameraOffset = GetComponent<CinemachineCameraOffset>();

            Transform following = GetComponent<CinemachineVirtualCamera>().Follow;
            if (following == null || !following.TryGetComponent(out _rb)) Debug.LogError("Look module requires rigidbody to follow.", this);
        }

        private void Update()
        {
            _input = SolveInput();
            _multiplier  = IsResettingOffset() ? resetMultiplier : _input.magnitude;
            _destination = IsResettingOffset() ? Vector2.zero    : _input.normalized * radius;

        }
        
        // ReSharper disable once ConvertIfStatementToReturnStatement
        private Vector2 SolveInput()
        {
            // Solve gamepad.
            Vector2 gamepad = GameController.LookInput.Gamepad;
            if (gamepad.magnitude > 0f) return gamepad;

            // Solve keyboard.
            Vector2 keyboard = GameController.LookInput.Cardinal + GameController.LookInput.Diagonal.Rotate(z: 45f);
            if (keyboard.magnitude > 0f) return keyboard;
            
            return Vector2.zero;
        }

        private void FixedUpdate()
        {
            switch (Vector2.Distance(_cameraOffset.m_Offset, _destination))
            {
                case 0f:
                    return;
                case <= 0.001f:
                    _cameraOffset.m_Offset = _destination;
                    break;
                default:
                    _cameraOffset.m_Offset = Vector2.Lerp(_cameraOffset.m_Offset, _destination, _multiplier * speed * Time.deltaTime);
                    break;
            }
        }


        /// <summary>
        /// Camera is currently offset.
        /// </summary>
        /// <returns>True if camera is currently offset. False otherwise.</returns>
        public bool IsOffset()
        {
            return Vector2.Distance(_cameraOffset.m_Offset, Vector2.zero) != 0f;
        }

        /// <summary>
        /// Resetting camera offset.
        /// </summary>
        /// <returns>True if currently changing offset to zero. False otherwise.</returns>
        public bool IsResettingOffset()
        {
            return GameController.LookInput.Reset || _rb.velocity != Vector2.zero;
        }
    }
}
