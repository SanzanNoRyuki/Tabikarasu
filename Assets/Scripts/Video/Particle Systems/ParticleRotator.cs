using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleRotator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f;

    [Range(-360f, 360f)]
    [SerializeField]
    private float initialRotationOffset = -0f;

    private ParticleSystem _particleSystem;
    private Vector2        _previousPosition;

    private void Awake()
    {
        _particleSystem   = GetComponent<ParticleSystem>();
        _previousPosition = transform.position;

    }

    private void Update()
    {
        RotateParticleEffect();
        _previousPosition = transform.position;
    }

    private void RotateParticleEffect()
    {

        Vector2 movementDirection = ((Vector2)transform.position - _previousPosition).normalized;

        if (movementDirection != Vector2.zero)
        {
            float      angle          = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;


            // Consider the initial -90 degrees rotation
            angle += initialRotationOffset;
            
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            _particleSystem.transform.rotation = Quaternion.RotateTowards(_particleSystem.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}