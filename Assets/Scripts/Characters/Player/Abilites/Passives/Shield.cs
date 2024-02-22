using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player.Skills.Elemental.Passives
{
    /// <summary>
    /// Basic shield blocking one complete instance of damage except instant kills.
    /// </summary>
    public class Shield : Passive
    {
        [SerializeField]
        [Range(0.1f, 10f)]
        [Tooltip("Regeneration delay.")]
        private float regenerationDelay = 5f;
        private float _lastTimeBroken;

        [SerializeField]
        private UnityEvent shielded;
        public event Action Shielded;

        [SerializeField]
        private UnityEvent broken;
        public event Action Broken;
    
        private Coroutine _regenerationCoroutine;
        
        public  bool      IsBroken => !enabled || Time.time - _lastTimeBroken <= regenerationDelay;

        private void OnEnable()
        {
            if (!IsBroken) return;

            _lastTimeBroken = 0f;
            shielded.Invoke();
            Shielded?.Invoke();

            Debug.Log("Shield enabled");
        }

        private void OnDisable()
        {
            if (IsBroken) return;

            _lastTimeBroken = regenerationDelay;
            broken.Invoke();
            Broken?.Invoke();

            Debug.Log("Shield disabled");
        }

        [ContextMenu("Break")]
        public void Break()
        {
            if (IsBroken) return;

            _lastTimeBroken = Time.time;
            broken.Invoke();
            Broken?.Invoke();

            if (_regenerationCoroutine != null) StopCoroutine(_regenerationCoroutine);
            _regenerationCoroutine = StartCoroutine(Regenerate());
        }
        
        private IEnumerator Regenerate()
        {
            while (_lastTimeBroken > 0f)
            {
                _lastTimeBroken -= Time.deltaTime;
                yield return null;
            }

            _regenerationCoroutine = null;
            shielded.Invoke();
            Shielded?.Invoke();
        }
    }
}
