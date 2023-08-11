using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utility.Level;
using Utility.Scenes;

public class TextTransition : MonoBehaviour
{
    public TextMeshProUGUI[] textBlocks;
    public float             fadeDuration = 1.0f;

    private int         currentIndex = 0;
    private CanvasGroup canvasGroup;
    private bool        waitForButtonPress = true;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(TransitionText());
    }

    private IEnumerator TransitionText()
    {
        while (true)
        {
            // Set the current text block to active and fade it in
            textBlocks[currentIndex].gameObject.SetActive(true);
            yield return FadeCanvasGroup(canvasGroup, 0f, 1f, fadeDuration);

            // Wait for any button press
            waitForButtonPress = true;
            yield return new WaitUntil(() => waitForButtonPress);

            // Fade out the current text block
            yield return FadeCanvasGroup(canvasGroup, 1f, 0f, fadeDuration);

            // Set the current text block to inactive
            textBlocks[currentIndex].gameObject.SetActive(false);

            // Move to the next text block
            currentIndex = (currentIndex + 1) % textBlocks.Length;

            if (currentIndex == 0)
            {
                SceneManager.Next();
            }
        }
    }




    
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            canvasGroup.alpha =  alpha;
            time              += Time.deltaTime;
            yield return null;
        }

        // Ensure that the final alpha is set correctly
        canvasGroup.alpha = endAlpha;
    }

    public void OnButtonPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            waitForButtonPress = false;
        }
    }
}