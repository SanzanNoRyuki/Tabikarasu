using System;
using System.Collections;
using System.Collections.Generic;
using Attacks.Affectables;
using Environment.Decorations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utility.Extensions;

public class CorpseDestroyer : MonoBehaviour
{
    [SerializeField]
    private LayerMask targets;

    [SerializeField]
    private UnityEvent onCorpseDestroyed;

    public event Action OnCorpseDestroyed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyCorpses(collision.Other());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DestroyCorpses(other.Other());
    }

    private void DestroyCorpses(GameObject target)
    {
        if (!targets.Contains(target.layer)) return;
        if (!target.TryGetComponent(out Corpse corpse)) return;

        corpse.Despawn();
        onCorpseDestroyed?.Invoke();
        OnCorpseDestroyed?.Invoke();
    }
}