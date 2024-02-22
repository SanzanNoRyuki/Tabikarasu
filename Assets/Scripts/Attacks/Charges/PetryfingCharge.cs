using Attacks.Affectables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks.Charges
{
    public class PetryfingCharge : Charge
    {
        [Range(0f, 60f)]
        [SerializeField]
        private float duration = 2f;

        
        protected override void OnHit(GameObject target, Vector2 direction)
        {
            if (target.TryGetComponent<Petrifiable>(out var freezable)) freezable.Petrify(duration);
        }
        
    }
}