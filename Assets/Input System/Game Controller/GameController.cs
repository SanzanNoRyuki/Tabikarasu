using System;
using Characters.Player.Abilites;
using Characters.Player.Skills;
using Characters.Player.Skills.Elemental;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility.Singletons;

namespace Input_System.Game_Controller
{
    /// <summary>
    /// Player input handler.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class GameController : PersistentSingleton<GameController>
    {
        /// <summary>
        /// Player is currently transforming, limiting his action range.
        /// </summary>
        public static bool IsTransforming { get; private set; }

        private PlayerInput _playerInput;
        private InputAction _moveInput;
        private InputAction _lookGamepad;
        private InputAction _lookCardinal;
        private InputAction _lookDiagonal;
        private InputAction _lookReset;

        /// <summary>
        /// <inheritdoc cref="IMoveInput"/>
        /// </summary>
        public static IMoveInput MoveInput => new MoveInputInternal();

        /// <summary>
        /// <inheritdoc cref="ILookInput"/>
        /// </summary>
        public static ILookInput LookInput => new LookInputInternal();

        private static PlayerInput PlayerInput
        {
            get
            {
                GameController instance = Instance != null ? Instance : FindObjectOfType<GameController>(true);
                if (instance == null) return null;

                return instance._playerInput != null ? instance._playerInput : instance._playerInput = instance.GetComponent<PlayerInput>();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _playerInput  = GetComponent<PlayerInput>();
            _moveInput    = PlayerInput.actions[Actions.Move];

            _lookGamepad  = PlayerInput.actions[Actions.LookGamepad];
            _lookCardinal = PlayerInput.actions[Actions.LookCardinal];
            _lookDiagonal = PlayerInput.actions[Actions.LookDiagonal];
            _lookReset    = PlayerInput.actions[Actions.LookReset];
        }

        protected override void OnApplicationQuit()
        {
            if (PlayerInput != null)
            {
                foreach (InputAction action in PlayerInput.actions) action.Dispose();
            }

            base.OnApplicationQuit();
        }

        /// <summary>
        /// Activate player input.
        /// </summary>
        public static void Activate()
        {
            if (PlayerInput != null) PlayerInput.ActivateInput();
        }

        /// <summary>
        /// Deactivate player input.
        /// </summary>
        public static void Deactivate()
        {
            if (PlayerInput != null) PlayerInput.DeactivateInput();
        }

        /// <summary>
        /// Subscribe to an action.
        /// </summary>
        /// <param name="actionName">Action to subscribe to.</param>
        /// <param name="callback">Method subscribed.</param>
        public static void Subscribe(string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (PlayerInput != null)
            {
                PlayerInput.actions[actionName].performed += callback;
            }
            else
            {
                Debug.LogError($"Subscription of {callback.Method} in {callback.Target} has failed.");
            }
        }

        /// <summary>
        /// Unsubscribe from an action.
        /// </summary>
        /// <param name="actionName">Action to unsubscribe from.</param>
        /// <param name="callback">Method to unsubscribe.</param>
        public static void Unsubscribe(string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (PlayerInput != null)
            {
                PlayerInput.actions[actionName].performed -= callback;
            }
            else
            {
                if (!ApplicationIsQuitting) Debug.LogError($"Unsubscription of {callback} has failed.");
            }
        }

        /// <summary>
        /// Disable actions not allowed during <see cref="PlayerElement">transforming</see>.
        /// </summary>
        public static void StartTransforming()
        {
            if (PlayerInput == null) return;
            PlayerInput.actions[Actions.Jump].Disable();
            PlayerInput.actions[Actions.Dash].Disable();
            PlayerInput.actions[Actions.Spell].Disable();
            PlayerInput.actions[Actions.Suicide].Disable();
            PlayerInput.actions[Actions.Interact].Disable();
            PlayerInput.actions[Actions.Pause].Disable();
            IsTransforming = true;
        }

        /// <summary>
        /// Restore actions not allowed during <see cref="PlayerElement">transforming</see>.
        /// </summary>
        public static void StopTransforming()
        {
            if (PlayerInput == null) return;
            PlayerInput.actions[Actions.Jump].Enable();
            PlayerInput.actions[Actions.Dash].Enable();
            PlayerInput.actions[Actions.Spell].Enable();
            PlayerInput.actions[Actions.Suicide].Enable();
            PlayerInput.actions[Actions.Interact].Enable();
            PlayerInput.actions[Actions.Pause].Enable();
            IsTransforming = false;
        }

        /// <summary>
        /// Input system actions.
        /// </summary>
        public static class Actions
        {
            // Movement:
            public const string Move = "Move";
            public const string Jump = "Jump";
            public const string Dash = "Dash";

            // Skills:
            public const string Spell    = "Spell";
            public const string Suicide  = "Suicide";
            public const string Interact = "Interact";

            // Transform:
            public const string Transform    = "Transform";
            public const string AirElement   = "AirElement";
            public const string WaterElement = "WaterElement";
            public const string EarthElement = "EarthElement";
            public const string FireElement  = "FireElement";
            public const string PrevElement  = "PreviousElement";

            // Look:    
            public const string LookCursor   = "LookCursor";
            public const string LookGamepad  = "LookGamepad";
            public const string LookCardinal = "LookCardinal";
            public const string LookDiagonal = "LookDiagonal";
            public const string LookReset    = "LookReset";

            // UI:
            public const string Pause = "Pause";
        }

        /// <summary>
        /// Input system action maps.
        /// </summary>
        public static class ActionMaps
        {
            public const string Game = "Game";
            public const string UI   = "UI";
        }

        private class MoveInputInternal : IMoveInput
        {
            public Vector2 Value => Instance == null || IsTransforming ? Vector2.zero : Instance._moveInput?.ReadValue<Vector2>() ?? Vector2.zero;
        }

        private class LookInputInternal : ILookInput
        {
            public Vector2 Gamepad  => Instance == null ? Vector2.zero : Instance._lookGamepad?.ReadValue<Vector2>()  ?? Vector2.zero;
            public Vector2 Cardinal => Instance == null ? Vector2.zero : Instance._lookCardinal?.ReadValue<Vector2>() ?? Vector2.zero;
            public Vector2 Diagonal => Instance == null ? Vector2.zero : Instance._lookDiagonal?.ReadValue<Vector2>() ?? Vector2.zero;
            public bool    Reset    => Instance != null && (Instance._lookReset?.IsPressed() ?? false);
        }
    }
}