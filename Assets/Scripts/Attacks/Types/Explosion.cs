using System;
using System.Collections.Generic;
using System.Linq;
using Attacks;
using Attacks.Affectables;
using Characters.Healths;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Explosive object.
/// </summary>
public class Explosion : Attack
{
    /// <summary>
    /// Minimum explosion range.
    /// </summary>
    public const float MinRadius = 0f;

    /// <summary>
    /// Maximum explosion range.
    /// </summary>
    public const float MaxRadius = 100f;

    /// <summary>
    /// Minimum explosion force.
    /// </summary>
    public const float MinForce = 0f;

    /// <summary>
    /// Maximum explosion force.
    /// </summary>
    public const float MaxForce = 100000f;

    /// <summary>
    /// Minimum explosion damage.
    /// </summary>
    public const int MinDamage = 0;

    /// <summary>
    /// Maximum explosion damage.
    /// </summary>
    public const int MaxDamage = 1000;

    [SerializeField]
    [Range(MinRadius, MaxRadius)]
    private float radius;

    [SerializeField]
    [Range(MinForce, MaxForce)]
    [Tooltip("Physical force applied to hit objects.")]
    private float force;

    [SerializeField]
    [Range(MinDamage, MaxDamage)]
    [Tooltip("Explosion damage applied to hit objects.")]
    private int damage;

    [SerializeField]
    [Range(0f, 10f)]
    [Tooltip("Explosion delay.")]
    private float delay;

    private Tween _delayedCallTween;

    private readonly List<Rigidbody2D> _bodies = new();

    /// <summary>
    /// Explosion radius.
    /// </summary>
    public float Radius
    {
        get => radius;
        set => radius = Mathf.Clamp(value, MinRadius, MaxRadius);
    }

    /// <summary>
    /// Explosion radius
    /// </summary>
    public float Force
    {
        get => force;
        set => force = Mathf.Clamp(value, MinForce, MaxForce);
    }

    /// <summary>
    /// Explosion radius
    /// </summary>
    public int Damage
    {
        get => damage;
        set => damage = Mathf.Clamp(value, MinDamage, MaxDamage);
    }

    private void OnEnable()
    {
        //DOVirtual.DelayedCall(delay, Explode).Override(ref _delayedCallTween);
        //Explode();
    }

    private void OnDisable()
    {
        //_delayedCallTween.Kill();
    }

    /// <summary>
    /// Make explosion explode and disable itself.
    /// </summary>
    [ContextMenu("Explode")]
    public void Explode()
    {
        // ReSharper disable once Unity.PreferNonAllocApi | Because it is deprecated.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Radius, Targets);


        _bodies.Clear();
        foreach (Collider2D hit in hits)
        {
            if (hit.attachedRigidbody != null) _bodies.Add(hit.attachedRigidbody);
        }

        foreach (Rigidbody2D body in _bodies.Distinct())
        {
            Debug.Log(body.name);

            Vector3 direction = body.transform.position - transform.position;
            body.AddForce(direction * Force, ForceMode2D.Impulse);

            if (body.TryGetComponent(out Health health)) health.Damage(Damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    public override event Action<GameObject, Vector2> OnHit;
}