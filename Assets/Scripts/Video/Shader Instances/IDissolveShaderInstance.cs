using DG.Tweening;
using UnityEngine;
using Video.Shader_Instances;

public interface IDissolveShaderInstance
{
    public float Dissolve        { get; set; }
    public Tween m_DissolveTween { get; set; }

    public void Set(float endValue, float duration = 0f)
    {
        m_DissolveTween.Kill();

        if (Mathf.Approximately(duration, 0f))
        {
            Dissolve = endValue;
        }
        else
        {
            m_DissolveTween = DOTween.To(() => Dissolve, value => Dissolve = value, endValue, Mathf.Max(0f, duration));
        }
    }

    public void Appear(float duration = 0f)
    {
        Set(ShaderInstance.MaxProgress, Mathf.Max(0f, duration));
    }

    public void Disappear(float duration = 0f)
    {
        Set(ShaderInstance.MinProgress, Mathf.Max(0f, duration));
    }

    /// <summary>
    /// Kill active tween stopping current changes.
    /// </summary>
    public void KillTween()
    {
        m_DissolveTween?.Kill();
    }

    public static readonly int DissolveReference            = Shader.PropertyToID("_Dissolve");
    public static readonly int DissolveBorderReference      = Shader.PropertyToID("_Dissolve_Border");
    public static readonly int DissolveBorderColorReference = Shader.PropertyToID("_Dissolve_Border_Color");
    public static readonly int DissolveNoiseReference       = Shader.PropertyToID("_Dissolve_Noise");
    public static readonly int DissolveNoiseScaleReference  = Shader.PropertyToID("_Dissolve_Noise_Scale");
}