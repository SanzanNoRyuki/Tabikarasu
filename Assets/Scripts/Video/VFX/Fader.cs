using DG.Tweening;
using UnityEngine;
using Utility.Extensions;

namespace Utility.Miscellaneous
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Fader : MonoBehaviour
    {
        [SerializeField]
        private bool hideOnStart = true;

        [Range(0f, 100f)]
        [SerializeField]
        [Tooltip("Duration of fade in and fade out.")]
        private float duration = 0.1f;

        private SpriteRenderer _sr;
        private Tween          _delayedCallTween;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (hideOnStart)
            {
                Color color = _sr.color;
                color.a   = 0f;
                _sr.color = color;
            }
        }

        /// <summary>
        /// Shows the object.
        /// </summary>
        [ContextMenu("Show")]
        public void Show()
        {
            _sr.DOFade(1f, duration).Override(ref _delayedCallTween);
        }

        /// <summary>
        /// Hides the object for a specified duration.
        /// </summary>
        [ContextMenu("Hide")]
        public void Hide()
        {
            _sr.DOFade(0f, duration).Override(ref _delayedCallTween);
        }

        /// <summary>
        /// Shows the object for a specified duration.
        /// </summary>
        public void Show(float shownDuration)
        {
            Show();
            _delayedCallTween = DOVirtual.DelayedCall(shownDuration, Hide);
        }

        /// <summary>
        /// Shows the object for a specified duration.
        /// </summary>
        public void Hide(float hideDuration)
        {
            Hide();
            _delayedCallTween = DOVirtual.DelayedCall(hideDuration, Show);
        }
    }
}
