using Characters.Behaviours;
using UnityEngine;
using Utility.Extensions;

[RequireComponent(typeof(Target))]
public class Turret : MonoBehaviour
{
    [SerializeField]
    [Range(0f, float.PositiveInfinity)]
    [Tooltip("Turret reacting range.")]
    private float range = float.PositiveInfinity;

    [SerializeField]
    private float lookAhead = 1f;

    [SerializeField]
    private float lowerLimit = -90f;

    [SerializeField]
    private float upperLimit = 90f;

    [SerializeField]
    private float speed = 1f;

    private Target _target;
    
    private void Awake()
    {
        _target = GetComponent<Target>();

        _target.Acquired += Enable;
        _target.Changed  += Enable;
        _target.Lost     += Disable;
    }

    private void OnDestroy()
    {
        _target.Acquired -= Enable;
        _target.Changed  -= Enable;
        _target.Lost     -= Disable;
    }

    private void Enable()
    {
        enabled = true;
    }

    private void Disable()
    {
        enabled = false;
    }

    private void Update()
    {
        var target = _target.Transform;

        if (!float.IsPositiveInfinity(range))
        {
            if (target == null || Vector2.Distance(transform.position, target.position) > range) return;
        }

        if (target == null) return;

        Vector2 direction = target.position - transform.position;

        if (lookAhead > 0f) direction += target.GetComponent<Rigidbody2D>().velocity * lookAhead;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle = Mathf.Clamp(angle, lowerLimit, upperLimit);

        var rotation  = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}