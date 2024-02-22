using System;
using DG.Tweening;
using UnityEngine;

namespace Video.Shader_Instances
{
    /// <summary>
    /// Interface for disintegration shader instance.
    /// </summary>
    public interface IDisintegrationShaderInstance
    {
        /// <summary>
        /// Disintegration progress shader reference.
        /// </summary>
        public static readonly int DisintegrationReference            = Shader.PropertyToID("_Disintegration");
        
        /// <summary>
        /// Disintegration border shader reference.
        /// </summary>
        public static readonly int DisintegrationBorderReference      = Shader.PropertyToID("_Disintegration_Border");

        /// <summary>
        /// Disintegration border color shader reference.
        /// </summary>
        public static readonly int DisintegrationBorderColorReference = Shader.PropertyToID("_Disintegration_Border_Color");

        /// <summary>
        /// Disintegration noise scale shader reference.
        /// </summary>
        public static readonly int DisintegrationNoiseScaleReference  = Shader.PropertyToID("_Disintegration_Noise_Scale");
        
        /// <summary> 
        /// Disintegration progress. 0 - fully disintegrated, 1 - fully integrated.
        /// </summary>
        public float Disintegration { get; set; }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Obsolete]
        #pragma warning disable IDE1006 // Named this way to discourage the use outside of this interface.
        public Tween m_DisintegrationTween { get; set; }
        #pragma warning restore IDE1006

        /// <summary>
        /// Set disintegration progress over time.
        /// </summary>
        /// <param name="endValue">Value at the end of duration.</param>
        /// <param name="duration">How long disintegration takes.</param>
        public void Set(float endValue, float duration = 0f)
        {
            KillTween();

            if (Mathf.Approximately(duration, 0f))
            {
                Disintegration = endValue;
            }
            else
            {
                #pragma warning disable CS0612
                m_DisintegrationTween = DOTween.To(() => Disintegration, value => Disintegration = value, endValue, Mathf.Max(0f, duration));
                #pragma warning restore CS0612
            }
        }

        /// <summary>
        /// Appear over time.
        /// </summary>
        /// <param name="duration"></param>
        public void Appear(float duration = 0f)
        {
            Set(ShaderInstance.MaxProgress, Mathf.Max(0f, duration));
        }

        /// <summary>
        /// Disappear over time.
        /// </summary>
        /// <param name="duration"></param>
        public void Disappear(float duration = 0f)
        {
            Set(ShaderInstance.MinProgress, Mathf.Max(0f, duration));
        }

        /// <summary>
        /// Kill active tween stopping current changes.
        /// </summary>
        public void KillTween()
        {
            #pragma warning disable CS0612
            m_DisintegrationTween?.Kill();
            #pragma warning restore CS0612
        }
    }
}