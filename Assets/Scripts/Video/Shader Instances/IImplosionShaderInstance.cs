using DG.Tweening;
using UnityEngine;
using Video.Shader_Instances;

public interface IImplosionShaderInstance
{
    public float Implosion        { get; set; }
    public Tween m_ImplosionTween { get; set; }

    public void Set(float endValue, float duration = 0f)
    {
        m_ImplosionTween.Kill();

        if (Mathf.Approximately(duration, 0f))
        {
            Implosion = endValue;
        }
        else
        {
            m_ImplosionTween = DOTween.To(() => Implosion, value => Implosion = value, endValue, Mathf.Max(0f, duration));
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
        m_ImplosionTween?.Kill();
    }

    public static readonly int ImplosionReference            = Shader.PropertyToID("_Implosion");
    public static readonly int ImplosionBorderReference      = Shader.PropertyToID("_Implosion_Border");
    public static readonly int ImplosionBorderColorReference = Shader.PropertyToID("_Implosion_Border_Color");
}