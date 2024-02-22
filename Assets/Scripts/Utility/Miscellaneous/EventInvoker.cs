using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Utility.Miscellaneous
{
    /// <summary>
    /// Repeatedly invokes given event.
    /// </summary>
    public class EventInvoker : MonoBehaviour
    {
        /// <summary>
        /// Minimum period duration.
        /// </summary>
        public const float MinPeriod = 0.01f;

        /// <summary>
        /// Maximum period duration.
        /// </summary>
        public const float MaxPeriod = 100f;

        /// <summary>
        /// Default period duration.
        /// </summary>
        public const float DefaultPeriod = 0.5f;

        /// <summary>
        /// Default tick rate.
        /// </summary>
        public const float DefaultTickRate = 1f / DefaultPeriod;

        [Header("Timing")]

        [SerializeField]
        [Range(MinPeriod, MaxPeriod)]
        [Tooltip("How many seconds passes between ticks.\nInverse of tick rate.")]
        private float period = DefaultPeriod;
        private float _previousPeriod;
        
        [SerializeField]
        [Range(MinPeriod, MaxPeriod)]
        [Tooltip("How many ticks happens per second.\nInverse of period.")]
        private float tickRate = DefaultTickRate;
        private float _previousTickRate;

        [Header("Probability")]

        [SerializeField]
        [Range(0.01f, 1f)]
        [Tooltip("Probability event gets invoked each tick.")]
        private float probability = 0.33f;
        private float _probabilityAccumulator;
        
        [SerializeField]
        [Tooltip("Accumulate fail probability, raising odds for continuous ticks.")]
        private bool accumulate;

        private float _timeCounter;

        [SerializeField]
        [Tooltip("Invoked event.")]
        private UnityEvent @event;

        /// <summary>
        /// Invoked event.
        /// </summary>
        public event Action Event;

        private void Awake()
        {
            _previousTickRate = tickRate;
            _previousPeriod = period;
        }

        private void OnValidate()
        {
            // Update tick rate and period according to newest value.
            if (!Mathf.Approximately(tickRate, _previousTickRate))
            {
                _previousTickRate = tickRate;
                _previousPeriod   = period = 1f / tickRate;
            }
            else if (!Mathf.Approximately(period, _previousPeriod))
            {
                _previousPeriod   = period;
                _previousTickRate = tickRate = 1f / period;
            }
        }

        private void Update()
        {
            if (_timeCounter >= period)
            {
                Tick();
                _timeCounter = 0f;
            }
            else
            {
                _timeCounter += Time.deltaTime;
            }
        }

        /// <summary>
        /// Invoke event from outside.
        /// </summary>
        [ContextMenu("Invoke")]
        public void Invoke()
        {
            _probabilityAccumulator = 0f;
            @event.Invoke();
            Event?.Invoke();
        }

        /// <summary>
        /// <see cref="Invoke()">Invoke</see> with a delay. Can't be cancelled.
        /// </summary>
        /// <param name="delay">Invocation delay.</param>
        public void Invoke(float delay)
        {
            DOVirtual.DelayedCall(delay, Invoke);
        }

        private void Tick()
        {
            if (accumulate)
            {
                _probabilityAccumulator += probability;
            }
            else
            {
                _probabilityAccumulator = probability;
            }

            if (Random.value <= _probabilityAccumulator) Invoke();
        }
    }
}