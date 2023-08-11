using UnityEngine;
using Utility.Attributes;
using Utility.Singletons;

namespace Input_System
{
    /// <summary>
    /// Cursor controller handling cursor interactions.
    /// </summary>
    public class CursorController : PersistentSingleton<CursorController>
    {
        [Fixed]
        [SerializeField]
        [Tooltip("Enable cursor on start.")]
        private bool enabledOnStart = true;

        private void Start()
        {
            if (enabledOnStart)
            {
                EnableCursor();
            }
            else
            {
                DisableCursor();
            }
        }

        /// <summary>
        /// Activates and shows cursor.
        /// </summary>
        public static void EnableCursor()
        {
            Cursor.visible   = true;
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Deactivates and hides cursor.
        /// </summary>
        public static void DisableCursor()
        {
            Cursor.visible   = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        /// <summary>
        /// Hides cursor but keeps it active.
        /// </summary>
        public static void HideCursor()
        {
            Cursor.visible = false;
        }

        /// <summary>
        /// Check if cursor is currently active.
        /// </summary>
        /// <returns>True if cursor is enabled. False otherwise.</returns>
        public static bool IsCursorActive()
        {
            return Cursor.lockState == CursorLockMode.None;
        }

        [ContextMenu("Enable Cursor")]
        private void EnableCursorContextMenu()
        {
            EnableCursor();
        }

        [ContextMenu("Disable Cursor")]
        private void DisableCursorContextMenu()
        {
            DisableCursor();
        }

        [ContextMenu("Hide Cursor")]
        private void HideCursorContextMenu()
        {
            HideCursor();
        }
    }
}