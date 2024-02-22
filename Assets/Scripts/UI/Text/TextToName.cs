#if UNITY_EDITOR
using TMPro;
using UnityEngine;

namespace UI.Text
{
    /// <summary>
    /// Updates game object name to text.
    /// </summary>
    /// <remarks>
    /// Editor only component.
    /// </remarks>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    [RequireComponent(typeof(Transform))]
    public sealed class TextToName : MonoBehaviour
    {
        // Update is only called when something in the Scene changed
        private void Update()
        {
            // Editor only component
            if (Application.isPlaying) enabled = false;
            else transform.name                = GetComponent<TMP_Text>().text;
        }
    }
}
#endif