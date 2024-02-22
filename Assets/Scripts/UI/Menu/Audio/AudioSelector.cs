using Audio;
using Miscellaneous.Management;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// Add sound to a selectable UI object.
    /// </summary>
    [DisallowMultipleComponent]
    public class AudioSelector : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        [Tooltip("Sound to play when the selectable UI object is selected.")]
        private Sound sound;

        /// <summary>
        /// Do this when the selectable UI object is selected.
        /// </summary>
        /// <param name="eventData">Event payload associated with pointer (mouse / touch) events.</param>
        public void OnSelect(BaseEventData eventData)
        {
            PlaySelect();
        }

        /// <summary>
        /// Play the sound.
        /// </summary>
        [ContextMenu("Play Select")]
        public void PlaySelect()
        {
            SoundManager.Instance.PlayAsUI(sound);
        }
    }
}