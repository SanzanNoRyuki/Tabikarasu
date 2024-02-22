using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Attacks;
using Attacks.Abilities;
using Environment.Decorations;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using Utility.Attributes;
using Utility.Object_Pooling;

public class Projectile : Attack, IPoolableObject
{
    public float Speed = 10f;

    [SerializeField]
    private bool respectRotation = true;

    [SerializeField]
    private float lifetime = 10f;

    private WaitForSeconds _lifetime;

    [Fixed]
    [SerializeField]
    [Tooltip("Object spawned on death.")]
    private Corpse corpse;
    private Corpse _corpse;

    [SerializeField]
    [Tooltip("Time after despawn before projectile is returned to pool.")]
    private float despawnDelay = 5f;

    [SerializeField]
    private UnityEvent onSpawn;

    private Shooter _shooter;

    private Vector2 Direction { get; set; }
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lifetime = new WaitForSeconds(lifetime);

        if (corpse != null)
        {
            _corpse = Instantiate(corpse);
            _corpse.gameObject.SetActive(false);
        }
    }

    public void Shoot(Vector3 direction, [CanBeNull] Shooter shooter = null)
    {
        if (respectRotation && direction != transform.right) transform.right = direction;


        Direction    = direction;
        _rb.velocity = Direction * Speed;

        _shooter = shooter;
        if (lifetime != 0)
        {
            StartCoroutine(DespawnAfter());
        }

        Spawn();
        if (_corpse != null) _corpse.Despawn();
    }



    private IEnumerator DespawnAfter()
    {
        yield return _lifetime;

        Despawn(true);
    }

    public override event Action<GameObject, Vector2> OnHit;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!IsValidTarget(other.collider, out GameObject go)) return;

        OnHit?.Invoke(go, Direction);
        Despawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsValidTarget(other, out GameObject go)) return;

        OnHit?.Invoke(go, Direction);
        Despawn();
        
    }

    public event Action Spawned;

    
    public void Spawn()
    {
        Spawned?.Invoke();
        onSpawn?.Invoke();
    }

    public void Despawn()
    {
        Despawn(true);
    }

    public void Despawn(bool corpse)
    {
        if (_shooter != null) _shooter.Release(this, despawnDelay);

        gameObject.SetActive(false);
        if (corpse && _corpse != null) _corpse.Spawn(transform.position, transform.rotation);
    }
}

