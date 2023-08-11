using DG.Tweening;
using UnityEngine;

namespace Video.VFX
{
    /// <summary>
    /// Makes object pulsing continuously.
    /// </summary>
    [DisallowMultipleComponent]
    public class Pulsar : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("True scale without manipulation.")]
        private Vector3 trueScale = Vector3.one;

        [SerializeField]
        [Range(0f, 2f)]
        [Tooltip("Scale multiplier during pulse.")]
        private float sizeMultiplier = 1.1f;

        [SerializeField]
        [Range(0f, 5f)]
        [Tooltip("Single pulse duration.")]
        private float duration = 0.5f;

        private Tween _pulse;

        private void Awake()
        {
            trueScale = transform.localScale;
        }

        private void OnEnable()
        {
            ResetProperties();
        }

        private void OnDisable()
        {
            _pulse.Kill();
        }

        private void ResetProperties()
        {
            _pulse.Kill();
            _pulse = transform.DOScale(trueScale * sizeMultiplier, duration).From(trueScale).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
