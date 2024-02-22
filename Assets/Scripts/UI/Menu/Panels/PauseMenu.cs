using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public sealed class PauseMenu : MonoBehaviour
    {
        private Sequence showSequence;
        private Sequence slideSequence;

        // Current anchor position, or final position after tweens finish.
        private Vector2 currentPosition;
        private Vector2 defaultPosition;

        private RectTransform rectTransform;
        private Vector2       referenceResolution;


        private void Awake()
        {
            showSequence = DOTween.Sequence();
            slideSequence = DOTween.Sequence();

            rectTransform = GetComponent<RectTransform>();

            referenceResolution = GetComponentInParent<Canvas>().GetComponent<CanvasScaler>().referenceResolution;

            currentPosition = defaultPosition = rectTransform.anchoredPosition;
        }

        /*
        private void OnEnable()
        {
            throw new NotImplementedException();
        }*/

        public void Open()
        {
            // Enable Pause menu
            // Resetni na start
            // Set default select ! not the same
            // Move menu down

            gameObject.SetActive(true);

            // StopInternal hiding
            showSequence.Kill();
            showSequence.Append(rectTransform.DOAnchorPos(Vector2.zero, 0.5f));
            currentPosition = Vector2.zero;


            //rectTransform.DOAnchorPos(Vector2.zero, 0.5f);
            gameObject.SetActive(true);


        }



        public float ShowDuration { get; private set; } = 1f;

        public void Close()
        {
            showSequence.Kill();
            showSequence.Append(rectTransform.DOAnchorPosY(1080f, 5f)
                                             .OnComplete(() => gameObject.SetActive(false)));

            ;
            showSequence.Append(rectTransform.DOAnchorPosY(1080f, ShowDuration)).OnComplete(Restore);
        }

        public void Restore()
        {
            gameObject.SetActive(false);
            rectTransform.anchoredPosition = defaultPosition;
        }

        public void SlideRight()
        {
            Slide(Vector2.right);
        }

        public void SlideLeft()
        {
            Slide(Vector2.left);
        }

        public void SlideUp()
        {
            Slide(Vector2.up);
        }

        public void SlideDown()
        {
            Slide(Vector2.down);
        }

        public void Slide(Vector2 direction)
        {
            currentPosition += referenceResolution * direction;

            // Sequence prevents misaligning
            slideSequence.Append(rectTransform.DOAnchorPos(currentPosition, 1f, true));
        }

        private void Update()
        {
            Debug.Log(currentPosition);
        }
    }
}