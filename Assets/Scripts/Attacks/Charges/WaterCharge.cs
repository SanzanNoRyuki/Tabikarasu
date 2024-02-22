using Attacks.Affectables;
using UnityEngine;

namespace Attacks.Charges
{
    /// <summary>
    /// Charge capable of making <see cref="Splashable"/> objects wet.
    /// </summary>
    public class WaterCharge : Charge
    {
        protected override void OnHit(GameObject target, Vector2 direction)
        {
            if (target.TryGetComponent<Splashable>(out var splashable)) splashable.Splash();
        }
    }
}
