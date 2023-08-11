using UnityEngine;
using UnityEngine.Audio;
using Utility.Attributes;
using Utility.Scenes;
using Utility.Singletons;

namespace Audio
{
    /// <summary>
    /// Audio manager handling most of the audio interactions in the game.
    /// </summary>
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        [Fixed]
        [SerializeField]
        [Tooltip("Audio mixer used by the game.")]
        private AudioMixer masterMixer;

        [Header("Default Sources")]

        [Fixed]
        [SerializeField]
        [Tooltip("Default audio source for sound effects.")]
        private AudioSource sounds;

        [Fixed]
        [SerializeField]
        [Tooltip("Default audio source for UI sounds.")]
        private AudioSource userInterface;

        /// <summary>
        /// Audio mixer used by the game.
        /// </summary>
        public static AudioMixer MasterMixer => Instance == null ? null : Instance.masterMixer;

        protected override void Awake()
        {
            base.Awake();

            if (MasterMixer   == null) Debug.LogError($"{nameof(MasterMixer)} audio mixer is not assigned.",    this);
            if (sounds        == null) Debug.LogError($"{nameof(sounds)} audio source is not assigned.",        this);
            if (userInterface == null) Debug.LogError($"{nameof(userInterface)} audio source is not assigned.", this);
        }

        private void OnEnable()
        {
            SceneManager.PreSceneChange  += DisableSounds;
            SceneManager.PostSceneChange += EnableSounds;
        }

        private void OnDisable()
        {
            SceneManager.PreSceneChange  -= DisableSounds;
            SceneManager.PostSceneChange -= EnableSounds;
        }

        public static void PlaySound(Sound sound)
        {
            if (Instance == null) return;

            sound.Play(Instance.sounds);
        }

        public static void PlaySound(Sound sound, Vector3 position)
        {
            sound.Play(position);
        }

        public void PlayAsUI(Sound sound)
        {
            sound.Play(userInterface);
        }

        public static void EnableSounds()
        {
            if (Instance    == null) return;
            if (MasterMixer == null) return;

            SetVolume(MixerGroups.Ambiance, GroupsVolume.Ambiance);
            SetVolume(MixerGroups.SFX,      GroupsVolume.SFX);
        }

        public static void DisableSounds()
        {
            if (Instance    == null) return;
            if (MasterMixer == null) return;

            GroupsVolume.Ambiance = SetVolume(MixerGroups.Ambiance, -80f);
            GroupsVolume.SFX      = SetVolume(MixerGroups.SFX,      -80f);
        }

        private static float SetVolume(string parameterName, float volume)
        {
            if (MasterMixer.GetFloat(parameterName, out float currentVolume))
            {
                if (Mathf.Approximately(currentVolume, volume)) return currentVolume;
                MasterMixer.SetFloat(parameterName, volume);
            }
            else
            {
                Debug.LogWarning($"{parameterName} was not found", Instance);
            }

            return currentVolume;
        }

        private static class MixerGroups
        {
            public const  string Ambiance = "Ambiance";
            public const  string Master   = "Master";
            public const  string Music    = "Music";
            public const  string SFX      = "SFX";
            public const  string UI       = "UI";
        }

        private static class GroupsVolume
        {
            public static float Ambiance;
            public static float SFX;
        }
    }
}