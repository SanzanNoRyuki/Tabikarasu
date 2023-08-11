using JetBrains.Annotations;
using UnityEngine;

namespace Audio.Sound_Managers
{
    /// <summary>
    /// Object sound manager.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class LocalSoundManager : MonoBehaviour
    {
        private AudioSource _source;

        protected virtual void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Play sound as <see cref="AudioSource.PlayOneShot(AudioClip)">one shot</see> from object's position.
        /// </summary>
        /// <param name="sound">Sound to be played. If sound is <i>null</i> does nothing.</param>
        public void Play([CanBeNull] Sound sound = null)
        {
            if (sound != null) sound.Play(_source);
        }

        /// <summary>
        /// Start playing a sound loop at this object's position.
        /// </summary>
        /// <param name="sound">Sound to be played. If sound is <i>null</i> stops current sound loop.</param>
        public void StartLoop([CanBeNull] Sound sound = null)
        {
            if (sound != null)
            {
                sound.StartLoop(_source);
            }
            else
            {
                StopLoop();
            }
        }

        /// <summary>
        /// Stops current sound loop.
        /// </summary>
        public void StopLoop()
        {
            Sound.StopLoop(_source);
        }
    }
}