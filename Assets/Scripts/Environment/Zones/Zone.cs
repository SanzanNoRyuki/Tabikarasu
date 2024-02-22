using System;
using System.Collections.Generic;
using System.Linq;
using Environment.Interactive.Doors;
using UnityEngine;
using UnityEngine.Events;
using Utility.Constants;
using Utility.Extensions;

namespace Environment.Zones
{
    /// <summary>
    /// Checks conditions on collision/trigger enter.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    [DisallowMultipleComponent]
    public class Zone : MonoBehaviour
    {
        [SerializeField]
        private List<ProgressCondition> conditions;
    
        [SerializeField]
        private UnityEvent<GameObject> onConditionsMet;
        public event Action<GameObject> ConditionsMet;

        [SerializeField]
        private UnityEvent<GameObject> onConditionsNotMet;
        public event Action<GameObject> OnConditionsNotMet;

        private void OnCollisionEnter2D(Collision2D otherCollision)
        {
            CheckConditions(otherCollision.collider);
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            CheckConditions(otherCollider);
        }

        private void CheckConditions(Collider2D otherCollider)
        {
            if (otherCollider.isTrigger) return;

            GameObject other = otherCollider.Other();
            
            if (!other.CompareTag(Tags.Player)) return;

            if (CheckConditions())
            {
                onConditionsMet.Invoke(other);
                ConditionsMet?.Invoke(other);
            }
            else
            {
                onConditionsNotMet.Invoke(other);
                OnConditionsNotMet?.Invoke(other);
            }
        }

        public bool CheckConditions()
        {
            return conditions.All(condition => condition.ConditionMet());
        }
    }
}