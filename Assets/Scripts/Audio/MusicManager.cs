using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Singletons;

namespace Audio
{
    /// <summary>
    /// Music manager playing music tracks based on the current scene.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : PersistentSingleton<MusicManager>
    {
        [SerializeField]
        [Tooltip("List of music tracks ordered by scene.")]
        private List<Sound> musicTracks;

        private AudioSource _source;
        private Coroutine   _trackSwapCoroutine;

        protected override void Awake()
        {
            base.Awake();
            
            _source = GetComponent<AudioSource>();
            
            while (musicTracks.Count < SceneManager.sceneCountInBuildSettings) musicTracks.Add(null);

            if (musicTracks[0] != null) musicTracks[0].StartLoop(_source);
        }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnSceneChange;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        /// <summary>
        /// Pause music.
        /// </summary>
        public static void Pause()
        {
            if (Instance != null) Instance._source.Pause();
        }

        /// <summary>
        /// Resume music.
        /// </summary>
        public static void Resume()
        {
            if (Instance != null) Instance._source.UnPause();
        }
        
        [ContextMenu("Pause Music")]
        private void PauseContextMenu()
        {
            Pause();
        }

        [ContextMenu("Resume Music")]
        private void ResumeContextMenu()
        {
            Resume();
        }

        private Sound Track(Scene scene)
        {
            return musicTracks[Mathf.Clamp(scene.buildIndex, 0, musicTracks.Count - 1)];
        }

        private void ChangeTrack(Sound sound, float duration = 0f)
        {
            if (_trackSwapCoroutine != null) StopCoroutine(_trackSwapCoroutine);
            
            _trackSwapCoroutine = StartCoroutine(ChangingTrack(sound, duration));
        }

        private void OnSceneChange(Scene previous, Scene current)
        {
            if (Track(previous) != Current()) ChangeTrack(Track(current), Utility.Scenes.SceneManager.TransitionDuration);

        }

        private Sound Current()
        {
            return Track(SceneManager.GetActiveScene());
        }
        
        private IEnumerator ChangingTrack(Sound sound, float duration)
        {
            Tween myTween = _source.DOFade(0, duration);

            yield return myTween.WaitForCompletion();

            if (sound != null)
            {
                sound.StartLoop(_source);
                _source.DOFade(sound.Volume, duration);
            }
            else
            {
                _source.Pause();
            }
        }
    }
}