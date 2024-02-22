using System;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Sound class storing audio data.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Sound")]
    public class Sound : ScriptableObject
    {
        /// <summary>
        /// Minimum sound volume.
        /// </summary>
        public const float MinVolume = 0.01f;

        /// <summary>
        /// Maximum sound volume.
        /// </summary>
        public const float MaxVolume = 1f;

        /// <summary>
        /// Volume used when playing this sound.
        /// </summary>
        [field: Range(MinVolume, MaxVolume)]
        [field: SerializeField]
        [field: Tooltip("Volume used when playing this sound.")]
        public float Volume { get; private set; } = 1f;
        
        [SerializeField]
        [Tooltip("Audio clip to play.")]
        private AudioClip clip;

        /// <summary>
        /// Plays sound as <see cref="AudioSource.PlayOneShot(AudioClip)">one shot</see> from <see cref="SoundManager"/>.
        /// </summary>
        [ContextMenu("Play")]
        public void Play()
        {
            SoundManager.PlaySound(this);
        }

        /// <summary>
        /// Plays sound as <see cref="AudioSource.PlayOneShot(AudioClip)">one shot</see> from given source.
        /// </summary>
        /// <remarks>
        /// Tries to use absolute <see cref="Volume"/> instead of scale.
        /// </remarks>
        /// <param name="source">Source to play <see cref="AudioClip"/> from.</param>
        public void Play(AudioSource source)
        {
            if (clip == null) return;
            source.volume = Mathf.Max(source.volume, MinVolume);
            source.PlayOneShot(clip, Volume * (1 / source.volume));
        }

        /// <summary>
        /// Plays sound as <see cref="AudioSource.PlayClipAtPoint(AudioClip,Vector3)">one shot</see> from given position.
        /// </summary>
        /// <param name="position">Position at which sound should be played.</param>
        public void Play(Vector3 position)
        {
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, position, Volume);
        }

        /// <summary>
        /// Starts playing a sound loop from given source.
        /// </summary>
        /// <param name="source">Source loop to play from.</param>
        public void StartLoop(AudioSource source)
        {
            if (source == null) return;

            if (clip != null)
            {
                source.clip   = clip;
                source.volume = Volume;
                source.loop   = true;
                source.Play();
            }
            else
            {
                StopLoop(source);
            }
        }

        /// <summary>
        /// Stops current sound loop.
        /// </summary>
        /// <param name="source">Source loop is playing from.</param>
        public static void StopLoop(AudioSource source)
        {
            if (source == null) return;
            if (!source.isPlaying) return;
            
            source.clip   = null;
            source.volume = MaxVolume;
            source.Pause();
        }
    }
}