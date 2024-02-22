using System;
using Audio;
using Audio.Sound_Managers;
using UnityEngine;
using Utility.Extensions;

namespace Attacks.Charges
{
    /// <summary>
    /// Plays sound on hit. Does nothing if target/owner doesn't have <see cref="LocalSoundManager"/>.
    /// </summary>
    public class AudioCharge : Charge
    {
        [SerializeField]
        [Tooltip("If target centered audio charge is played from the target.\nPlayed from the owner otherwise.")]
        private bool targetCentered = true;

        [SerializeField]
        private Sound sound;

        private LocalSoundManager _soundManager;

        private void Awake()
        {
            this.TryGetRelatedComponent(out _soundManager);
        }

        protected override void OnHit(GameObject target, Vector2 direction)
        {
            LocalSoundManager soundManager = targetCentered ? target.GetRelatedComponent<LocalSoundManager>() : _soundManager;

            if (soundManager != null) soundManager.Play(sound);
        }
    }
}