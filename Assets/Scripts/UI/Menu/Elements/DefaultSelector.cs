using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menu
{
    /// <summary>
    /// Default <see cref="UnityEngine.UI.Selectable">selectable</see> on enable.
    /// </summary>
    [DisallowMultipleComponent]
    public class DefaultSelector : MonoBehaviour
    {
        /// <summary>
        /// Selectable reference.
        /// </summary>
        [field: SerializeField]
        public Selectable Selectable { get; private set; } = default;

        private void OnEnable()
        {
            DefaultSelect();
        }

        /// <summary>
        /// Selects <see cref="Selectable">selectable</see> at the end of the frame.
        /// </summary>
        /// <remarks>
        /// Delay is required, because OnEnable() is called too early in the order of execution
        /// </remarks>
        public void DefaultSelect()
        {
            StartCoroutine(DefaultSelectAtTheEndOfTheFrame());
        }

        private IEnumerator DefaultSelectAtTheEndOfTheFrame()
        {
            yield return new WaitForEndOfFrame();

            // Reset currently selected - fixes already selected issues
            EventSystem.current.SetSelectedGameObject(null);

            Selectable.Select();
        }
    }
}