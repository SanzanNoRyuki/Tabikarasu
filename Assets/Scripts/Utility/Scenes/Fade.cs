using DG.Tweening;
using UnityEngine;

namespace Utility.Scenes
{
    /// <summary>
    /// Fade <see cref="Transition"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public class Fade : Transition
    {
        /// <summary>
        /// Canvas alpha when active. Overrides <see cref="CanvasGroup.alpha"/>.
        /// </summary>
        [field: Range(0f, 1f)]
        [field: SerializeField]
        public float ActiveAlpha { get; private set; } = 1f;

        [ContextMenu("Activate")]
        protected override void Activate()
        {
            base.Activate();

            CanvasGroup.alpha = ActiveAlpha;
        }

        [ContextMenu("Deactivate")]
        protected override void Deactivate()
        {
            base.Deactivate();

            CanvasGroup.alpha = 0f;
        }

        public override void Show(float duration)
        {
            if (duration == 0f)
                Activate();
            else
            {
                CanvasGroup.DOFade(ActiveAlpha, duration).OnComplete(Activate);
            }
        }

        public override void Hide(float duration)
        {
            if (duration == 0f)
                Deactivate();
            else
                CanvasGroup.DOFade(0f, duration).OnComplete(Deactivate);
        }
    }
}