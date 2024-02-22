using System.Collections.Generic;
using Characters.Behaviours;
using UnityEngine;
using UnityEngine.Events;
using Utility.Constants;
using Utility.Extensions;

namespace Characters.AI.Targeting {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerColliderFinder : MonoBehaviour
    {
        private Target target;

        private Collider2D searchCollider;

        private bool flipX;

        private UnityEvent hasTarget;

        private void Awake()
        {
            searchCollider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            GameObject other = collision.Other();

            if (other != null && other.CompareTag(Tags.Player))
            {
                hasTarget?.Invoke();
                target.Set(other.transform);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            GameObject other = collision.Other();

            if (other.transform == target.Transform) target.Set(null);
        }
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Target))]
    public class PlayerHomeDefender : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 15f)]
        [Tooltip("Searches for player in a circular area around home.")]
        private float range;

        [SerializeField]
        [Tooltip("Home location. Spawn point by default.")]
        private Vector3 home;

        private Target _target;

        private void Awake()
        {
            if (home == Vector3.zero) home = transform.position;

            playerFilter.SetLayerMask(Layers.Player);
        }

        private void Update()
        {
            if (_target.HasTarget)
            {
                LoseTarget();
            }
            else
            {
                FindTarget();
            }

        }

        private List<Collider2D> hits = new List<Collider2D>();

        private ContactFilter2D playerFilter = new ContactFilter2D();

        private void FindTarget()
        {
            hits.Clear();
            Physics2D.OverlapCircle(home, range, playerFilter, hits);

            foreach (Collider2D hit in hits)
            {
                _target.Set(hit.Other().transform);
                break;
            }
        }

        private void LoseTarget()
        {
            if (Vector2.Distance(home, _target.Transform.position) > range) _target = null;
        }
    }

    public class TargetAttacker : MonoBehaviour
    {
        [SerializeField]
        private float checkRate = 1f;
        private float lastCheckTime = 0f;

        private Ability ability;

        private Target target;

        private void Awake()
        {
            target = GetComponent<Target>();
        }

        private void Update()
        {
            if (Time.time - lastCheckTime > checkRate)
            {
                lastCheckTime = Time.time;

                Check();
            }
        }

        private void Check()
        {
            if (!target.HasTarget) return;

        
            ability.Cast();

            // DO THIS IN AOE
            GetComponent<Rigidbody2D>().constraints = GetComponent<Rigidbody2D>().constraints | RigidbodyConstraints2D.FreezePositionX;
        }
    }
}