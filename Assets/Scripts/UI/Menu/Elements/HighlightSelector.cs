using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menu
{
    /// <summary>
    /// Selects this object on pointer enter.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    public class HighlightSelector : MonoBehaviour, IPointerEnterHandler
    {
        private Selectable selectable;

        private void Awake()
        {
            selectable = GetComponent<Selectable>();
        }

        /// <summary>
        /// Do this when the cursor enters the rect area of this selectable UI object.
        /// </summary>
        /// <param name="eventData">Event payload associated with pointer (mouse / touch) events.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            selectable.Select();
        }
    }
}