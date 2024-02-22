using System.Collections;
using DG.Tweening;
using Miscellaneous.Quotes;
using UnityEngine;

namespace UI.Text
{
    /// <summary>
    /// Dynamically replaces text field with values from string generator.
    /// </summary>
    public class GeneratedText : DynamicText
    {
        private Sequence sequence;

        private Coroutine swapTextRoutine;

        /// <summary>
        /// Generator filling text field
        /// </summary>
        [field: SerializeField]
        public Generator Generator { get; private set; }

        /// <summary>
        /// Transition duration in seconds.
        /// </summary>
        [field: Range(0f, 3f)]
        [field: SerializeField]
        public float Transition { get; private set; } = 0.025f;

        /// <summary>
        /// Stay duration in seconds.
        /// </summary>
        [field: Range(1f, 10f)]
        [field: SerializeField]
        public float StayDuration { get; private set; } = 0.025f;
        public void Regenerate()
        {
            TextField.text = Generator.GenerateString();
        }






















        private void OnEnable()
        {
            swapTextRoutine = StartCoroutine(SwapText());
        }

        private void OnDisable()
        {
            //StopCoroutine(swapTextRoutine);
        }

        

        private IEnumerator SwapText()
        {
            TextField.alpha = 0f;

            while (true)
            {
                TextField.DOFade(1f, Transition);
                TextField.text = Generator.GenerateString();
                Debug.Log(TextField.text);
                yield return new WaitForSeconds(StayDuration);
            }


            /*
            field.alpha = 0f;

            sequence = DOTween.Sequence()
                              .Append(sequence = DOTween.Sequence()
                              .Append(field.DOFade(1f, Transition))
                              .SetDelay(StayDuration * 0.5f)
                              .SetLoops(-1, LoopType.Yoyo);)
                              .SetDelay(StayDuration * 0.5f)
                              .SetLoops(-1, LoopType.Yoyo);

            while (true)
            {
                // Fade In
                field.text = Generator.GenerateString();
                var fade = field.DOFade(1f, Transition);

                yield return fade.WaitForCompletion();
                yield return new WaitForSeconds(StayDuration);

                // Fade Out
                var fade2 = field.DOFade(0f, Transition);

                yield return fade2.WaitForCompletion();
            }
            */
        }
    }
}