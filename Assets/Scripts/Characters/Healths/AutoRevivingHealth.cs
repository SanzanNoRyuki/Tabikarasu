using System.Collections;
using Characters.LifeCycle;
using LifeCycle;
using UnityEngine;
using Utility.Attributes;
using Utility.Constants;
using Utility.Save_System;
using Utility.Save_System.Data;

namespace Characters.Healths
{
    /// <summary>
    /// Health that revives itself on death after delay.
    /// </summary>
    public class AutoRevivingHealth : Health, IPersistentDataHandler
    {
        [SerializeField]
        [FixedRange(0f, 100f)]
        [Tooltip("Time it takes for player to respawn.")]
        private float respawnDelay = 1f;
        private WaitForSeconds _respawnDelay;

        private Coroutine _respawnCoroutine;

        private bool _isPlayer;
        private static int _playerDeaths;

        protected override void Awake()
        {
            base.Awake();

            _isPlayer = gameObject.CompareTag(Tags.Player);

            _respawnDelay = new WaitForSeconds(respawnDelay);
        }

        /// <summary>
        /// <inheritdoc cref="Health.Kill"/>
        /// </summary>
        public override void Kill()
        {
            base.Kill();

            if (_isPlayer)_playerDeaths++;
            
            if (respawnDelay > 0f)
            {
                // Use of manager because it object itself gets disabled.
                if (_respawnCoroutine != null) LifeCycleManager.Instance.StopCoroutine(_respawnCoroutine);
                _respawnCoroutine = LifeCycleManager.Instance.StartCoroutine(Respawning());
            }
            else
            {
                Respawn();
            }
        }

        private IEnumerator Respawning()
        {
            yield return _respawnDelay;
            Respawn();
        }

        public void Save(ref GameData gameData)
        {
            gameData.Deaths = _playerDeaths;
        }

        public void Load(GameData gameData)
        {
            _playerDeaths = gameData.Deaths;
        }
    }
}