using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Detects spikes collision.
/// </summary>
public class SpikesDetector : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Invoked when spikes are detected.")]
    private UnityEvent onSpikes;

    /// <summary>
    /// Invoked when spikes are detected.
    /// </summary>
    public event Action OnSpikes;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetectSpikes(collision.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DetectSpikes(other.transform);
    }

    private void DetectSpikes(Transform target)
    {
        if (!target.root.TryGetComponent(out Spikes _)) return;

        onSpikes?.Invoke();
        OnSpikes?.Invoke();
    }
}
