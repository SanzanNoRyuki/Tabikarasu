using System;
using Characters.Healths;
using Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Attributes;
using Utility.Constants;
using Utility.Singletons;

namespace Characters.LifeCycle
{
    /// <summary>
    /// Player life cycle manager.
    /// </summary>
    public class LifeCycleManager : PersistentSingleton<LifeCycleManager>
    {
        /// <summary>
        /// Minimal amount of lives player can achieve.
        /// </summary>
        public const int MinLives = 1;

        /// <summary>
        /// Maximum amount of lives player can achieve.
        /// </summary>
        public const int MaxLives = 10;

        [SerializeField]
        [FixedRange(MinLives, MaxLives)]
        [Tooltip("How many lives player currently has.")]
        private int lives = 3;

        /// <summary>
        /// Whether life cycle is progressing.
        /// </summary>
        public static bool Ticking { get; set; } = true;

        private Health _player;
        private ElementComponent _element;

        /// <summary>
        /// Life cycle progressed because player has died.
        /// </summary>
        public static event Action Ticked;
        
        /// <summary>
        /// Life count has been increased.
        /// </summary>
        public static event Action LivesIncreased;

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            base.OnSceneLoaded(scene, mode);

            SetPlayer();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            if (_player != null) _player.Died -= Tick;
        }

        /// <summary>
        /// How many lives player currently has.
        /// </summary>
        public static int Lives()
        {
            return Instance != null ? Instance.lives : 0;
        }

        /// <summary>
        /// Increase life count by 1 up to <see cref="MaxLives"/>.
        /// </summary>
        public bool IncreaseLives()
        {
            if (lives < MaxLives)
            {
                lives++;
                LivesIncreased?.Invoke();
                return true;
            }
            
            return false;
        }

        private static void Tick()
        {
            if (Instance._element != null && Instance._element.Current == Element.Air) return;
            Ticked?.Invoke();
        }

        private void SetPlayer()
        {
            if (_player != null) _player.Died -= Tick;


            GameObject playerObject = GameObject.FindWithTag(Tags.Player);

            if (playerObject != null && playerObject.TryGetComponent(out Health health))
            {
                _player      =  health;
                _player.Died += Tick;
                _element = _player.GetComponent<ElementComponent>();
            }
            else
            {
                Debug.LogError("Player reference is not set and cannot be found in scene.", this);
            }
        }
    }
}
