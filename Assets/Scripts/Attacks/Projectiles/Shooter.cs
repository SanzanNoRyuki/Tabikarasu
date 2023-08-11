using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Attacks;
using Attacks.Abilities;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class Shooter : MonoBehaviour
{


    [SerializeField]
    private UnityEvent onShoot;

    [SerializeField]
    private int capacity;

    [SerializeField]
    private Projectile projectilePrefab;

    private ObjectPool<Projectile> _magazine;

    private void Awake()
    {
        _magazine = new ObjectPool<Projectile>(() => Instantiate(projectilePrefab), p => p.gameObject.SetActive(true), p => p.gameObject.SetActive(false), p => Destroy(p.gameObject), true, capacity, capacity);
    }


    [ContextMenu("Shoot")]
    public void Shoot()
    {

        Projectile instance = _magazine.Get();
        instance.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        instance.Shoot(transform.right, this);
        onShoot.Invoke();
    }

    // Draws a ray in the shoot direction.
    private void OnDrawGizmosSelected()
    {
        const float length       = 2f;
        
        var         transformVar = transform;
        var         direction    = transformVar.right;
        var         position     = transformVar.position;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(position, direction * length);

    }

    /*
    public override void Cast()
    {
        var instance = Instantiate(bullet, transform.position, Quaternion.identity);
        instance.Shoot(transform.right);
    }

    public override event Action<Rigidbody2D, Vector2> Hit;
    */

    public void Release(Projectile projectile, float delay = 5f)
    {
        DOVirtual.DelayedCall(delay, () => _magazine.Release(projectile));
    }
}