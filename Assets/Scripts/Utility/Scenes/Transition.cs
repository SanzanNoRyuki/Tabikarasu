using UnityEngine;

namespace Utility.Scenes
{
    /// <summary>
    /// Canvas group scene transition.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Transition : MonoBehaviour
    {
        protected CanvasGroup CanvasGroup { get; private set; }

        protected void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            Hide();
        }

        protected void OnEnable()
        {
            SceneManager.PreSceneChange  += Show;
            SceneManager.PostSceneChange += Hide;
        }
        
        protected void OnDisable()
        {
            SceneManager.PreSceneChange  -= Show;
            SceneManager.PostSceneChange -= Hide;
        }

        /// <summary>
        /// Activates transition, blocking scene view.
        /// </summary>
        protected virtual void Activate()
        {
            CanvasGroup                = GetComponent<CanvasGroup>();
            CanvasGroup.alpha          = 1f;
            CanvasGroup.interactable   = true;
            CanvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Deactivates transition, showing scene view.
        /// </summary>
        protected virtual void Deactivate()
        {
            CanvasGroup                = GetComponent<CanvasGroup>();
            CanvasGroup.alpha          = 0f;
            CanvasGroup.interactable   = false;
            CanvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// <inheritdoc cref="Show(float)"/>
        /// </summary>
        public void Show()
        {
            Show(SceneManager.TransitionDuration);
        }

        /// <summary>
        /// <inheritdoc cref="Hide(float)"/>
        /// </summary>
        public void Hide()
        {
            Hide(SceneManager.TransitionDuration);
        }

        /// <summary>
        /// Gradually block scene view.
        /// </summary>
        /// <param name="duration">Duration in seconds.</param>
        public abstract void Show(float duration);

        /// <summary>
        /// Gradually show scene view.
        /// </summary>
        /// <param name="duration">Duration in seconds.</param>
        public abstract void Hide(float duration);
    }
}