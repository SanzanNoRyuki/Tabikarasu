using System;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks.Types
{
    /// <summary>
    /// Area of effect attack.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class AOE : Attack
    {
        private Collider2D _hitBox;

        public override event Action<GameObject, Vector2> OnHit;

        private void Awake()
        {
            _hitBox = GetComponent<Collider2D>();
        }

        public void Cast()
        {
            var hits = new List<Collider2D>();
            Physics2D.OverlapCollider(_hitBox, new ContactFilter2D(), hits);


            foreach (Collider2D hit in hits)
            {
                Debug.Log(hit.name + IsValidTarget(hit, out GameObject _));

                if (IsValidTarget(hit, out GameObject go)) OnHit?.Invoke(go, transform.right);
            }
        }
    }
}
