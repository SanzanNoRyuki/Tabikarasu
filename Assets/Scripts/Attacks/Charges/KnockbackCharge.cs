using Attacks.Affectables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks.Charges
{
    public class KnockbackCharge : Charge
    {
        [Range(0f, 1000f)]
        [SerializeField]
        private float force = 10f;
        
        protected override void OnHit(GameObject target, Vector2 direction)
        {
            if (target.TryGetComponent(out Rigidbody2D rb)) rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
