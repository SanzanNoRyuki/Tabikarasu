using System;
using UnityEngine;
using Utility.Singletons;

namespace Utility.Miscellaneous
{
    /// <summary>
    /// Game time manager.
    /// </summary>
    public class TimeManager : PersistentSingleton<TimeManager>
    {
        /// <summary>
        /// Time is currently frozen.
        /// </summary>
        public static bool IsFrozen { get; private set; }

        /// <summary>
        /// Time was frozen.
        /// </summary>
        public static event Action Froze;

        /// <summary>
        /// Time was unfrozen.
        /// </summary>
        public static event Action Unfroze;

        /// <summary>
        /// Stops the time.
        /// </summary>
        public static void Freeze()
        {
            if (IsFrozen) return;

            IsFrozen       = true;
            Time.timeScale = 0f;
            Froze?.Invoke();
        }

        /// <summary>
        /// Unfreezes the time.
        /// </summary>
        public static void UnFreeze()
        {
            if (!IsFrozen) return;
        
            IsFrozen       = false;
            Time.timeScale = 1f;
            Unfroze?.Invoke();
        }

        /// <summary>
        /// Toggles the time freeze.
        /// </summary>
        public static void Toggle()
        {
            if (IsFrozen)
            {
                UnFreeze();
            }
            else
            {
                Freeze();
            }
        }

        [ContextMenu("Freeze")]
        private void FreezeContextMenu()
        {
            Freeze();
        }

        [ContextMenu("UnFreeze")]
        private void UnFreezeContextMenu()
        {
            UnFreeze();
        }
    }
}
