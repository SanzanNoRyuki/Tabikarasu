using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.Serialization;
using Utility.Extensions;

public class ColorChanger : MonoBehaviour
{
    [FormerlySerializedAs("initialColor")]
    [SerializeField]
    private Color primaryColor;
    
    [FormerlySerializedAs("targetColor")]
    [SerializeField]
    private Color secondaryColor;

    [SerializeField]
    private float duration;

    private Renderer _renderer;
    private Tween    _delayedCallTween;

    private void Awake()
    {
        _renderer                = GetComponent<Renderer>();
        _renderer.material.color = primaryColor;
    }

    public void ChangeColor(Color color)
    {
        _renderer.material.DOColor(color, duration).Override(ref _delayedCallTween);;
    }


    [ContextMenu("Primary Color")]
    public void PrimaryColor()
    {
        ChangeColor(primaryColor);
    }
    
    [ContextMenu("Secondary Color")]
    public void SecondaryColor()
    {
        ChangeColor(secondaryColor);
    }

    [ContextMenu("Swap Color")]
    public void SwapColor()
    {
        ChangeColor(_renderer.material.color == primaryColor ? secondaryColor : primaryColor);
    }

    [ContextMenu("Swap Color")]
    public void SwapColor(float swapDuration)
    {
        SwapColor();
        _delayedCallTween = DOVirtual.DelayedCall(swapDuration, ChangeColor);
    }
    

    [ContextMenu("Change Color")]
    public void ChangeColor()
    {
        //DOTweenAddons.DelayedTweenCleanup(ref _delayedCallTween);

        _renderer.material.DOColor(_renderer.material.color == primaryColor ? secondaryColor : primaryColor, duration);
    }

    public void ChangeColor(float changeDuration)
    {
        ChangeColor();
        _delayedCallTween = DOVirtual.DelayedCall(changeDuration, ChangeColor);
    }
}
