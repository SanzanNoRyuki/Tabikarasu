using DG.Tweening;
using UnityEngine;

namespace Video.Shader_Instances
{
    /// <summary>
    /// Shader instance for character shader.
    /// </summary>
    public class CharactersShaderInstance : ShaderInstance, IGlowShaderInstance, IImplosionShaderInstance, IDissolveShaderInstance, IDisintegrationShaderInstance, IPetrificationShaderInstance
    {
        // Color fields:
        
        [Header("Glow")]
        [SerializeField]
        [Range(MinProgress, MaxProgress)]
        [Tooltip("Glow progress. 0 = no glow, 1 = fully glowing.")]
        private float glow;

        [SerializeField]
        [ColorUsage(true, true)]
        [Tooltip("Glow color.")]
        private Color glowColor = Color.white;

        [Header("Petrification")]
        [SerializeField]
        [Range(MinProgress, MaxProgress)]
        [Tooltip("Petrification progress. 0 = no petrification, 1 = fully petrified.")]
        private float petrification;

        [SerializeField]
        [Range(MinBorder, MaxBorder)]
        [Tooltip("Petrification border width.")]
        private float petrificationBorder = 0.01f;

        [SerializeField]
        [ColorUsage(true, true)]
        [Tooltip("Petrification border color.")]
        private Color petrificationBorderColor = Color.gray;

        [SerializeField]
        [Range(MinNoiseScale, MaxNoiseScale)]
        [Tooltip("Petrification noise scale.")]
        private float petrificationNoiseScale = 25f;

        // Alpha fields:

        [Header("Implosion")]
        [SerializeField]
        [Range(MinProgress, MaxProgress)]
        [Tooltip("Implosion progress. 0 = fully imploded, 1 = no implosion.")]
        private float implosion = 1f;

        [SerializeField]
        [Range(MinBorder, MaxBorder)]
        [Tooltip("Implosion border width.")]
        private float implosionBorder = 0.02f;

        [SerializeField]
        [ColorUsage(true, true)]
        [Tooltip("Implosion border color.")]
        private Color implosionBorderColor = Color.white;

        [Header("Dissolve")]
        [SerializeField]
        [Range(MinProgress, MaxProgress)]
        [Tooltip("Dissolve progress. 0 = fully dissolved, 1 = no dissolve.")]
        private float dissolve = 1f;

        [SerializeField]
        [Range(MinBorder, MaxBorder)]
        [Tooltip("Dissolve border width.")]
        private float dissolveBorder = 0.1f;

        [SerializeField]
        [ColorUsage(true, true)]
        [Tooltip("Dissolve border color.")]
        private Color dissolveBorderColor = Color.white;

        [SerializeField]
        [Range(MinNoise, MaxNoise)]
        [Tooltip("Dissolve noise.")]
        private float dissolveNoise = 0.25f;

        [SerializeField]
        [Range(MinNoiseScale, MaxNoiseScale)]
        [Tooltip("Dissolve noise scale.")]
        private float dissolveNoiseScale = 75f;

        [Header("Disintegration")]
        [SerializeField]
        [Range(MinProgress, MaxProgress)]
        [Tooltip("Disintegration progress. 0 = fully disintegrated, 1 = no disintegration.")]
        private float disintegration = 1f;

        [SerializeField]
        [Range(MinBorder, MaxBorder)]
        [Tooltip("Disintegration border width.")]
        private float disintegrationBorder = 0.1f;

        [SerializeField]
        [ColorUsage(true, true)]
        [Tooltip("Disintegration border color.")]
        private Color disintegrationBorderColor = Color.white;

        [SerializeField]
        [Range(MinNoiseScale, MaxNoiseScale)]
        [Tooltip("Disintegration noise scale.")]
        private float disintegrationNoiseScale = 75f;

        // Internal instance tweens:
        public Tween m_GlowTween           { get; set; }
        public Tween m_ImplosionTween      { get; set; }
        public Tween m_DissolveTween       { get; set; }
        public Tween m_DisintegrationTween { get; set; }
        public Tween m_PetrificationTween  { get; set; }

        // Color properties:

        /// <summary>
        /// Glow progress. 0 = no glow, 1 = fully glowing.
        /// </summary>
        public float Glow
        {
            get => glow;
        
            set
            {
                if (Mathf.Approximately(value, glow)) return;
                glow = Mathf.Clamp(value, MinProgress, MaxProgress);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Glow color.
        /// </summary>
        public Color GlowColor
        {
            get => glowColor;

            set
            {
                if (value == glowColor) return;
                glowColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Petrification progress. 0 = no petrification, 1 = fully petrified.
        /// </summary>
        public float Petrification
        {
            get => petrification;

            set
            {
                if (Mathf.Approximately(value, petrification)) return;
                petrification = Mathf.Clamp(value, MinProgress, MaxProgress);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Petrification border width.
        /// </summary>
        public float PetrificationBorder
        {
            get => petrificationBorder;

            set
            {
                if (Mathf.Approximately(value, petrificationBorder)) return;
                petrificationBorder = Mathf.Clamp(value, MinBorder, MaxBorder);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Petrification border color.
        /// </summary>
        public Color PetrificationBorderColor
        {
            get => petrificationBorderColor;

            set
            {
                if (value == petrificationBorderColor) return;
                petrificationBorderColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Petrification noise scale.
        /// </summary>
        public float PetrificationNoiseScale
        {
            get => petrificationNoiseScale;

            set
            {
                if (Mathf.Approximately(value, petrificationNoiseScale)) return;
                petrificationNoiseScale = Mathf.Clamp(value, MinNoiseScale, MaxNoiseScale);
                OnPropertyChanged();
            }
        }

        // Alpha properties:

        /// <summary>
        /// Implosion progress. 0 = fully imploded, 1 = no implosion.
        /// </summary>
        /// <remarks>
        /// Use <see cref="SetImplosion(float, float)">SetImplosion()</see> to set implosion progressively.
        /// </remarks>
        public float Implosion
        {
            get => implosion;

            set
            {
                if (Mathf.Approximately(value, implosion)) return;
                implosion = Mathf.Clamp(value, MinProgress, MaxProgress);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Implosion border width.
        /// </summary>
        public float ImplosionBorder
        {
            get => implosionBorder;

            set
            {
                if (Mathf.Approximately(value, implosionBorder)) return;
                implosionBorder = Mathf.Clamp(value, MinBorder, MaxBorder);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Implosion border color.
        /// </summary>
        public Color ImplosionBorderColor
        {
            get => implosionBorderColor;

            set
            {
                if (value == implosionBorderColor) return;
                implosionBorderColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Dissolve progress. 0 = fully dissolved, 1 = no dissolve.
        /// </summary>
        /// <remarks>
        /// Use <see cref="SetDissolve(float, float)">SetDissolve()</see> to set dissolve progressively.
        /// </remarks>
        public float Dissolve
        {
            get => dissolve;

            set
            {
                if (Mathf.Approximately(value, dissolve)) return;
                dissolve = Mathf.Clamp(value, MinProgress, MaxProgress);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Dissolve border width.
        /// </summary>
        public float DissolveBorder
        {
            get => dissolveBorder;

            set
            {
                if (Mathf.Approximately(value, dissolveBorder)) return;
                dissolveBorder = Mathf.Clamp(value, MinBorder, MaxBorder);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Dissolve border color.
        /// </summary>
        public Color DissolveBorderColor
        {
            get => dissolveBorderColor;

            set
            {
                if (value == dissolveBorderColor) return;
                dissolveBorderColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Dissolve noise.
        /// </summary>
        public float DissolveNoise
        {
            get => dissolveNoise;

            set
            {
                if (Mathf.Approximately(value, dissolveNoise)) return;
                dissolveNoise = Mathf.Clamp(value, MinNoise, MaxNoise);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Dissolve noise scale.
        /// </summary>
        public float DissolveNoiseScale
        {
            get => dissolveNoiseScale;

            set
            {
                if (Mathf.Approximately(value, dissolveNoiseScale)) return;
                dissolveNoiseScale = Mathf.Clamp(value, MinNoiseScale, MaxNoiseScale);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Disintegration progress. 0 = fully disintegrated, 1 = no disintegration.
        /// </summary>
        /// <remarks>
        /// Use <see cref="SetDisintegration(float, float)">SetDisintegration()</see> to set disintegration progressively.
        /// </remarks>
        public float Disintegration
        {
            get => disintegration;

            set
            {
                if (Mathf.Approximately(value, disintegration)) return;
                disintegration = Mathf.Clamp(value, MinProgress, MaxProgress);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Disintegration border width.
        /// </summary>
        public float DisintegrationBorder
        {
            get => disintegrationBorder;

            set
            {
                if (Mathf.Approximately(value, disintegrationBorder)) return;
                disintegrationBorder = Mathf.Clamp(value, MinBorder, MaxBorder);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Disintegration border color.
        /// </summary>
        public Color DisintegrationBorderColor
        {
            get => disintegrationBorderColor;

            set
            {
                if (value == disintegrationBorderColor) return;
                disintegrationBorderColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Disintegration noise scale.
        /// </summary>
        public float DisintegrationNoiseScale
        {
            get => disintegrationNoiseScale;

            set
            {
                if (Mathf.Approximately(value, disintegrationNoiseScale)) return;
                disintegrationNoiseScale = Mathf.Clamp(value, MinNoiseScale, MaxNoiseScale);
                OnPropertyChanged();
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying) return;
            if (Material == null) return;

            KillTweens();
            ApplyChanges();
        }

        /// <summary>
        /// Set glow progress. 0 = no glow, 1 = full glow.
        /// </summary>
        /// <param name="value">Glow value.</param>
        public void SetGlow(float value)
        {
            SetGlow(value, 0f);
        }

        /// <summary>
        /// Set glow progress. 0 = no glow, 1 = full glow.
        /// </summary>
        /// <param name="value">Glow value.</param>
        /// <param name="duration">How long does change take.</param>
        public void SetGlow(float value, float duration)
        {
            ((IGlowShaderInstance) this).Set(value, duration);
        }

        /// <summary>
        /// Set implosion progress. 0 = fully imploded, 1 = no implosion.
        /// </summary>
        /// <param name="value">Implosion value.</param>
        public void SetImplosion(float value)
        {
            SetImplosion(value, 0f);
        }

        /// <summary>
        /// Set implosion progress. 0 = fully imploded, 1 = no implosion.
        /// </summary>
        /// <param name="value">Implosion value.</param>
        /// <param name="duration">How long does change take.</param>
        public void SetImplosion(float value, float duration)
        {
            ((IImplosionShaderInstance) this).Set(value, duration);
        }

        /// <summary>
        /// <inheritdoc cref="IImplosionShaderInstance.Appear"/>
        /// </summary>
        /// <param name="duration"><inheritdoc cref="IImplosionShaderInstance.Appear"/></param>
        public void ImplosionAppear(float duration = 0f)
        {
            ((IImplosionShaderInstance) this).Appear(duration);
        }

        public void ImplosionDissapear(float duration = 0f)
        {
            ((IImplosionShaderInstance) this).Disappear(duration);
        }

        // Dissolve interface:

        public void SetDissolve(float value)
        {
            SetDissolve(value, 0f);
        }

        public void SetDissolve(float value, float duration)
        {
            ((IDissolveShaderInstance) this).Set(value, duration);
        }

        public void DissolveAppear(float duration = 0f)
        {
            ((IDissolveShaderInstance) this).Appear(duration);
        }

        public void DissolveDissapear(float duration = 0f)
        {
            ((IDissolveShaderInstance) this).Disappear(duration);
        }

        // Disintegration interface:

        public void SetDisintegration(float value)
        {
            SetDisintegration(value, 0f);
        }

        public void SetDisintegration(float value, float duration)
        {
            ((IDisintegrationShaderInstance) this).Set(value, duration);
        }

        public void DisintegrationAppear(float duration = 0f)
        {
            ((IDisintegrationShaderInstance) this).Appear(duration);
        }

        public void DisintegrationDisappear(float duration = 0f)
        {
            ((IDisintegrationShaderInstance) this).Disappear(duration);
        }

        // Petrification interface:

        public void SetPetrification(float value)
        {
            SetPetrification(value, 0f);
        }

        public void SetPetrification(float value, float duration)
        {
            ((IPetrificationShaderInstance) this).Set(value, duration);
        }

        public void PetrifyCharacter(float duration = 0f)
        {
            ((IPetrificationShaderInstance) this).Petrify(duration);
        }

        public void UnpetrifyCharacter(float duration = 0f)
        {
            ((IPetrificationShaderInstance) this).Unpetrify(duration);
        }

        /// <summary>
        /// <inheritdoc cref="ShaderInstance.ApplyChanges()"/>
        /// </summary>
        protected override void ApplyChanges()
        {
            // Glow:
            Material.SetFloat(IGlowShaderInstance          .GlowsReference                    , Glow                     );
            Material.SetColor(IGlowShaderInstance          .GlowColorReference                , GlowColor                );

            // Implosion:
            Material.SetFloat(IImplosionShaderInstance     .ImplosionReference                , Implosion                );
            Material.SetFloat(IImplosionShaderInstance     .ImplosionBorderReference          , ImplosionBorder          );
            Material.SetColor(IImplosionShaderInstance     .ImplosionBorderColorReference     , ImplosionBorderColor     );

            // Dissolve:
            Material.SetFloat(IDissolveShaderInstance      .DissolveReference                 , Dissolve                 );
            Material.SetFloat(IDissolveShaderInstance      .DissolveBorderReference           , DissolveBorder           );
            Material.SetColor(IDissolveShaderInstance      .DissolveBorderColorReference      , DissolveBorderColor      );
            Material.SetFloat(IDissolveShaderInstance      .DissolveNoiseReference            , DissolveNoise            );
            Material.SetFloat(IDissolveShaderInstance      .DissolveNoiseScaleReference       , DissolveNoiseScale       );

            // Disintegration:
            Material.SetFloat(IDisintegrationShaderInstance.DisintegrationReference           , Disintegration           );
            Material.SetFloat(IDisintegrationShaderInstance.DisintegrationBorderReference     , DisintegrationBorder     );
            Material.SetColor(IDisintegrationShaderInstance.DisintegrationBorderColorReference, DisintegrationBorderColor);
            Material.SetFloat(IDisintegrationShaderInstance.DisintegrationNoiseScaleReference , DisintegrationNoiseScale );

            // Petrification:
            Material.SetFloat(IPetrificationShaderInstance .PetrificationReference            , Petrification            );
            Material.SetFloat(IPetrificationShaderInstance .PetrificationBorderReference      , PetrificationBorder      );
            Material.SetColor(IPetrificationShaderInstance .PetrificationBorderColorReference , PetrificationBorderColor );
            Material.SetFloat(IPetrificationShaderInstance .PetrificationNoiseScaleReference  , PetrificationNoiseScale  );
        }

        private void KillTweens()
        {
            ((IGlowShaderInstance) this).KillTween();
            ((IImplosionShaderInstance) this).KillTween();
            ((IDissolveShaderInstance) this).KillTween();
            ((IDisintegrationShaderInstance)this).KillTween();
            ((IPetrificationShaderInstance) this).KillTween();
        }
    }
}