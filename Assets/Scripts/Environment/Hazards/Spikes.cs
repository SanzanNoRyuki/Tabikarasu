using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Utility.Extensions;


/// <summary>
/// Extendable spikes.
/// </summary>
[SelectionBase]
[DisallowMultipleComponent]
public class Spikes : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Delay before spikes extension. Use to sync with animation.")]
    private float extensionDelay = 0.1f;

    [SerializeField]
    [Tooltip("Event raised on spikes extension.")]
    private UnityEvent onExtend;

    [SerializeField]
    [Tooltip("Event raised on spikes retraction.")]
    private UnityEvent onRetract;

    /// <summary>
    /// Event raised on spikes extension.
    /// </summary>
    public event Action OnExtend;

    /// <summary>
    /// Event raised on spikes retraction.
    /// </summary>
    public event Action OnRetract;

    private Tween _extension;

    private void Awake()
    {
        if (enabled) foreach (Collider2D coll in GetComponentsInChildren<Collider2D>()) coll.enabled = true;
        else         foreach (Collider2D coll in GetComponentsInChildren<Collider2D>()) coll.enabled = false;
    }


    private void OnEnable()
    {
        DOVirtual.DelayedCall(extensionDelay, () => { foreach (Collider2D coll in GetComponentsInChildren<Collider2D>()) coll.enabled = true; }).Override(ref _extension);
        
        onExtend.Invoke();
        OnExtend?.Invoke();
    }

    private void OnDisable()
    {
        _extension.Kill();
        foreach (Collider2D coll in GetComponentsInChildren<Collider2D>()) coll.enabled = false;
        onRetract.Invoke();
        OnRetract?.Invoke();
    }

    /// <summary>
    /// Toggles spikes.
    /// </summary>
    [ContextMenu("Toggle")]
    public void Toggle()
    {
        enabled = !enabled;
    }
}