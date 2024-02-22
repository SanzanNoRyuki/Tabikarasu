using UnityEngine;

namespace Input_System.Game_Controller
{
    /// <summary>
    /// Look input interface.
    /// </summary>
    public interface ILookInput
    {
        /// <summary>
        /// Gamepad look input.
        /// </summary>
        public Vector2 Gamepad { get; }

        /// <summary>
        /// Cardinal direction numpad look input.
        /// </summary>
        public Vector2 Cardinal { get; }

        /// <summary>
        /// Diagonal directions numpad look input.
        /// </summary>
        public Vector2 Diagonal { get; }

        /// <summary>
        /// Look default position reset input.
        /// </summary>
        public bool Reset { get; }
    }
}