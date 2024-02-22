using System.Collections.Generic;
using Miscellaneous.Movement;
using UnityEngine;
using Utility.Attributes;

namespace Environment.Interactive.Platforms
{
    /// <summary>
    /// Object that follows path of waypoints.
    /// </summary>
    public class WaypointFollower : Movement
    {
        [SerializeField]
        [Tooltip("Loop over waypoints.")]
        private bool loop = true;
        
        [SerializeField]
        [Range(MinSpeed, MaxSpeed)]
        [Tooltip("Speed of the object.")]
        private float speed = 5f;

        [ReadOnly]
        [SerializeField]
        [Tooltip("Current waypoint.")]
        private Transform current;

        [SerializeField]
        [Tooltip("Waypoints on the path.")]
        private List<Transform> waypoints = new();

        protected override void OnEnable()
        {
            base.OnEnable();

            if (EnforceWaypoints() && !current) current = waypoints[0];
        }

        private void OnValidate()
        {
            EnforceWaypoints();
        }

        private void Update()
        {
            if (ReachedWaypoint()) UpdateWaypoint();
        }

        private void FixedUpdate()
        {
            transform.position = Vector2.MoveTowards(transform.position, current.position, speed * Time.fixedDeltaTime);
        }

        private bool EnforceWaypoints()
        {
            if (waypoints.Count > 0) return true;

            Debug.LogWarning("Waypoint list is empty.", this);
            enabled = false;
            return false;
        }

        private bool ReachedWaypoint()
        {
            return Vector2.Distance(transform.position, current.position) < float.Epsilon;
        }

        private void UpdateWaypoint()
        {
            // If there is only one waypoint, do not update.
            if (waypoints.Count == 1)
            {
                enabled = false;
                return;
            }
            
            current = waypoints[(waypoints.IndexOf(current) + 1) % waypoints.Count];

            if (current)
            {
                if (!loop && waypoints.IndexOf(current) == 0) enabled = false;
            }
            else
            {
                Debug.LogWarning("Unassigned waypoint.", this);
                enabled = false;
            }
        }
    }
}
    