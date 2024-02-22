using System;
using System.Collections;
using UnityEngine;
using Utility.Extensions;

namespace Characters.Behaviours
{
    [DisallowMultipleComponent]
    public class TargetFlipper : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("How many seconds to wait before update.")]
        private float updateRate = 0.1f;
        private float _updateRateCounter;

        private Target _target;

        private void Awake()
        {
            _target = GetComponentInParent<Target>();

            if (_target == null)
            {
                Debug.LogError($"No {nameof(Target)} found in parent of {name}.", this);
            }
        }

        private void Update()
        {
            if (_updateRateCounter <= 0f)
            {
                Flip();
                _updateRateCounter = updateRate;
            }
            else
            {
                _updateRateCounter -= Time.deltaTime;
            }
        }

        private void Flip()
        {
            var target = _target.Transform;
            if (target == null) return;

            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Vector3 forwardDirection  = transform.right; // Assuming the object faces right initially

            // Calculate the dot product to check the direction
            float dotProduct = Vector3.Dot(forwardDirection, directionToTarget);
            if (dotProduct < 0f) transform.FlipX();
        }

    }
}