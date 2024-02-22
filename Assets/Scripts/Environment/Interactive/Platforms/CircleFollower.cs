using Miscellaneous.Movement;
using UnityEngine;
using Utility.Attributes;

namespace Environment.Interactive.Platforms
{
    /// <summary>
    /// Object that follows path of circle.
    /// </summary>
    public class CircleFollower : Movement
    {
        [SerializeField]
        private float speed = 5f;

        [SerializeField]
        [Tooltip("Center of the circle.")]
        private Transform center;

        [ReadOnly]
        [SerializeField]
        [Tooltip("Radius of the circle.")]
        private float radius;
        
        private float   _angle;
        private Vector2 _position;

        private float AngularSpeed => speed * Mathf.PI;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            RecalculateRadius();
        }
        
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            RecalculateRadius();
        }

        private void Update()
        {
            UpdatePosition();
            UpdateAngle();

            // Apply new position.
            transform.position = _position;
        }

        private void RecalculateRadius()
        {
            if (center)
            {
                radius = Vector2.Distance(transform.position, center.position);
            }
            else
            {
                Debug.LogWarning("Unassigned center.", this);
                enabled = false;
            }
        }

        private void UpdatePosition()
        {
            var centerPosition = (Vector2) center.position;
            var newX           = centerPosition.x + Mathf.Cos(_angle) * radius;
            var newY           = centerPosition.y + Mathf.Sin(_angle) * radius;

            _position = new Vector2(newX, newY);
        }

        private void UpdateAngle()
        {
            _angle = (_angle + AngularSpeed * Time.deltaTime) % 360f;
        }
    }
}