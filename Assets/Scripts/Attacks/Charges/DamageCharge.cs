using System;
using Attacks.Affectables;
using Characters.Healths;
using UnityEngine;
using Utility.Attributes;

namespace Attacks.Charges
{
    /// <summary>
    /// Applies damage on <see cref="Attack.OnHit">attack hit</see>.
    /// </summary>
    [DisallowMultipleComponent]
    public class DamageCharge : Charge
    {
        /// <summary>
        /// Damage to inflict on the target.
        /// </summary>
        [field: SerializeField]
        [field: Range(Health.MinDamage, Health.MaxDamage)]
        [field: Tooltip("Damage to inflict on the target.")]
        public int Damage { get; private set; } = 1;

        /// <summary>
        /// <inheritdoc cref="Charge.OnHit"/>
        /// </summary>
        /// <param name="target"><inheritdoc cref="Charge.OnHit"/></param>
        /// <param name="direction"><inheritdoc cref="Charge.OnHit"/></param>
        protected override void OnHit(GameObject target, Vector2 direction)
        {
            if (target.TryGetComponent(out Health health)) health.Damage(Damage);
        }
    }
}