using UnityEngine;

namespace Input_System.Game_Controller
{
    /// <summary>
    /// Movement input interface.
    /// </summary>
    public interface IMoveInput
    {
        /// <summary>
        /// Movement input.
        /// </summary>
        public Vector2 Value { get; }
    }
}