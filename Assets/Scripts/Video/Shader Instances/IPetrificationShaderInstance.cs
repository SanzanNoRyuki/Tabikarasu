using DG.Tweening;
using UnityEngine;
using Video.Shader_Instances;

public interface IPetrificationShaderInstance
{
    public float Petrification        { get; set; }
    public Tween m_PetrificationTween { get; set; }

    public void Set(float endValue, float duration = 0f)
    {
        m_PetrificationTween.Kill();

        if (Mathf.Approximately(duration, 0f))
        {
            Petrification = endValue;
        }
        else
        {
            m_PetrificationTween = DOTween.To(() => Petrification, value => Petrification = value, endValue, Mathf.Max(0f, duration));
        }
    }

    /// <summary>
    /// Kill active tween stopping current changes.
    /// </summary>
    public void KillTween()
    {
        m_PetrificationTween?.Kill();
    }

    public void Petrify(float duration = 0f)
    {
        Set(ShaderInstance.MaxProgress, Mathf.Max(0f, duration));
    }

    public void Unpetrify(float duration = 0f)
    {
        Set(ShaderInstance.MinProgress, Mathf.Max(0f, duration));
    }

    public static readonly int PetrificationReference            = Shader.PropertyToID("_Petrification");
    public static readonly int PetrificationBorderReference      = Shader.PropertyToID("_Petrification_Border");
    public static readonly int PetrificationBorderColorReference = Shader.PropertyToID("_Petrification_Border_Color");
    public static readonly int PetrificationNoiseScaleReference  = Shader.PropertyToID("_Petrification_Noise_Scale");
}