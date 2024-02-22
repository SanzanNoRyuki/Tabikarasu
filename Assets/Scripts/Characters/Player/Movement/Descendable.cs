using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Attributes;
using Utility.Extensions;

namespace Characters.Player.Movement
{
    /// <summary>
    /// This <see cref="Collider2D"/> can be descended by player.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Descendable : MonoBehaviour
    {
        [SerializeField]
        [FixedRange(0.01f, 1f)]
        [Tooltip("How often to check if the player is still inside this object.")]
        private float insideCheckRate = 0.3f;

        // Helper collection.
        private readonly List<RaycastHit2D> _hits = new(capacity: 1);

        private Collider2D      _collider;
        private WaitForSeconds  _insideCheckWaitTime;
        private ContactFilter2D _selfFilter;

        private void Awake()
        {
            _collider = GetComponent<CompositeCollider2D>();
            if (_collider == null) _collider = GetComponent<Collider2D>();

            
            _insideCheckWaitTime = new WaitForSeconds(insideCheckRate);
            _selfFilter.SetLayerMask(1 << gameObject.layer);
            _selfFilter.useTriggers = false;
        }

        /// <summary>
        /// Descend this platform.
        /// </summary>
        /// <param name="other">Descending object.</param>
        public void Descend(Rigidbody2D other)
        {
            StartCoroutine(Descending(other));
        }

        private IEnumerator Descending(Rigidbody2D other)
        {
            other.IgnoreCollision(_collider, true);
            
            while (true)
            {
                yield return _insideCheckWaitTime;

                // Reuse helper collection.
                _hits.Clear();
                other.Cast(Vector2.up, _selfFilter, _hits, float.Epsilon);

                if (_hits.All(hit => hit.collider != _collider)) break;
            }

            other.IgnoreCollision(_collider, false);
        }
    }
}