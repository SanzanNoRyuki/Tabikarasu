using DG.Tweening;
using Miscellaneous;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menu
{
    /// <summary>
    /// Emphasizes object on select by scaling it.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class Emphasize : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        [Tooltip("True scale without manipulation.")]
        private Vector3 trueScale;
        
        /// <summary>
        /// On select size multiplier.
        /// </summary>
        [field: Range(0f, 2f)]
        [field: SerializeField]
        public float ScaleMultiplier { get; private set; } = 1.05f;

        /// <summary>
        /// Transition duration in seconds.
        /// </summary>
        [field: Range(0f, 1f)]
        [field: SerializeField]
        public float Duration { get; private set; } = 0.025f;

        private void Awake()
        {
            trueScale = transform.localScale;
        }

        /// <summary>
        /// Do this when the selectable UI object is deselected.
        /// </summary>
        /// <param name="eventData">Event payload associated with pointer (mouse / touch) events.</param>
        public void OnDeselect(BaseEventData eventData)
        {
            transform.DOScale(trueScale, Duration);
        }

        /// <summary>
        /// Do this when the selectable UI object is selected.
        /// </summary>
        /// <param name="eventData">Event payload associated with pointer (mouse / touch) events.</param>
        public void OnSelect(BaseEventData eventData)
        {
            transform.DOScale(trueScale * ScaleMultiplier, Duration);
        }
    }
}