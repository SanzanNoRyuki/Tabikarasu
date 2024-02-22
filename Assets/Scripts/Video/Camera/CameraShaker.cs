using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Utility.Extensions;

namespace Video.Camera
{
    /// <summary>
    /// Camera shake for active camera.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShaker : MonoBehaviour
    {
        [Range(0f, 1000f)]
        [SerializeField]
        [Tooltip("Default duration of the shake.")]
        private float defaultDuration = 4f;
        
        [Range(0f, 1000f)]
        [SerializeField]
        [Tooltip("Default strength of the shake.")]
        private float defaultStrength = 50f;

        private CinemachineBasicMultiChannelPerlin _noise;
        private Tween                              _shakeTween;
        
        private static CameraShaker ActiveShaker
        {
            get
            {
                UnityEngine.Camera mainCamera = UnityEngine.Camera.main;
                
                // ReSharper disable ConvertIfStatementToReturnStatement
                if (mainCamera == null)                                                                          return null;
                if (!mainCamera.TryGetComponent(out CinemachineBrain brain))                                     return null;
                if (brain.ActiveVirtualCamera == null)                                                           return null;
                if (!brain.ActiveVirtualCamera.VirtualCameraGameObject.TryGetComponent(out CameraShaker shaker)) return null;
                // ReSharper restore ConvertIfStatementToReturnStatement
                return shaker;
            }
        }

        private void Awake()
        {
            _noise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (_noise != null)
            {
                _noise.m_AmplitudeGain = 0f;
            }
            else
            {
                Debug.LogError("Camera shake needs perlin noise component in camera.", this);
            }
        }

        /// <summary>
        /// Shake with default duration.
        /// </summary>
        public static void Shake()
        {
            ActiveShaker.OnShake();
        }

        /// <summary>
        /// Shake camera for the duration.
        /// </summary>
        /// <param name="duration">Duration of the shaking.</param>
        public static void Shake(float duration)
        {
            ActiveShaker.OnShake(duration);
        }

        /// <summary>
        /// Shake camera for the duration.
        /// </summary>
        /// <param name="duration">Duration of the shaking.</param>
        /// <param name="strength"></param>
        public static void Shake(float duration, float strength)
        {
            ActiveShaker.OnShake(duration, strength);
        }

        [ContextMenu("Shake with default duration and strength.")]
        private void OnShake()
        {
            OnShake(defaultDuration);
        }

        private void OnShake(float duration)
        {
            OnShake(duration, defaultStrength);
        }

        private void OnShake(float duration, float strength)
        {
            _noise.m_AmplitudeGain = strength;
            DOTween.To(() => _noise.m_AmplitudeGain, x => _noise.m_AmplitudeGain = x, 0f, duration).SetEase(Ease.OutExpo).OnComplete(() => _shakeTween = null).Override(ref _shakeTween);
        }
    }
}
