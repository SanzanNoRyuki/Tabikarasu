using DG.Tweening;
using UnityEngine;
using Video.Shader_Instances;

public interface IGlowShaderInstance
{
    
    /// <summary>
    /// Glow progress. 0 - no glow, 1 - max glow.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Set"/> to modify glow progress instead.
    /// </remarks>
    public float Glow { get; set; }

    /// <summary>
    /// Tween for glow progress. For internal use only.
    /// </summary>
    public Tween m_GlowTween { get; set; }

    /// <summary>
    /// Sets glow progress.
    /// </summary>
    /// <param name="value">Glow value at the end.</param>
    /// <param name="duration">How long transition takes.</param>
    public void Set(float value, float duration = 0f)
    {
        m_GlowTween.Kill();

        if (Mathf.Approximately(duration, 0f))
        {
            Glow = value;
        }
        else
        {
            m_GlowTween = DOTween.To(() => Glow, glow => Glow = glow, value, Mathf.Max(0f, duration));
        }
    }

    /// <summary>
    /// Set glow to max.
    /// </summary>
    /// <param name="duration"><inheritdoc cref="Set" path="/param[@name='duration']"/></param>
    public void On(float duration = 0f)
    {
        Set(ShaderInstance.MaxProgress, Mathf.Max(0f, duration));
    }

    /// <summary>
    /// Kill active tween stopping current changes.
    /// </summary>
    public void KillTween()
    {
        m_GlowTween?.Kill();
    }

    /// <summary>
    /// Set glow to min.
    /// </summary>
    /// <param name="duration"><inheritdoc cref="Set" path="/param[@name='duration']"/></param>
    public void Off(float duration = 0f)
    {
        Set(ShaderInstance.MinProgress, Mathf.Max(0f, duration));
    }

    public static readonly int GlowsReference     = Shader.PropertyToID("_Glows");
    public static readonly int GlowColorReference = Shader.PropertyToID("_Glow_Color");
}