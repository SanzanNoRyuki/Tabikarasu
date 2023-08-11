using DG.Tweening;
using UnityEngine;
using Utility.Extensions;

namespace Utility.Miscellaneous
{
    /// <summary>
    /// Handles object rotation. If enabled, rotates object continuously.
    /// </summary>
    [DisallowMultipleComponent]
    public class Rotor : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 100f)]
        [Tooltip("Single rotation duration.")]
        private float duration = 5f;

        private Tween _rotate;

        private void OnEnable()
        {
            StartRotating();
        }

        private void OnDisable()
        {
            _rotate.Kill();
        }

        [ContextMenu("Rotate")]
        public void Rotate()
        {
            Rotate(90f, 0f);
        }

        public void ForceRotation(float degrees)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, degrees);
        }

        public void Rotate(float degrees, float duration)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;

            if (duration <= 0f)
            {
                transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z + degrees);
            }
            else
            {
                transform.DORotate(new Vector3(currentRotation.x, currentRotation.y, currentRotation.z + degrees), duration, RotateMode.FastBeyond360)
                         .SetEase(Ease.Linear).OnComplete(RestoreRotation).Override(ref _rotate);
            }
        }

        private void StartRotating()
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            transform.DORotate(new Vector3(rotation.x, rotation.y, -360f), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).Override(ref _rotate);
        }

        private void RestoreRotation()
        {
            if (enabled) StartRotating();
        }

        
    }
}
