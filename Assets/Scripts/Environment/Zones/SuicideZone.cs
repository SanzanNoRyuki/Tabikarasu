using Attacks.Affectables;
using Characters.Healths;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Environment.Zones
{
    [DisallowMultipleComponent]
    public sealed class SuicideZone : Zone
    {
        private void OnEnable()
        {
            ConditionsMet += OnConditionsMet;
        }

        private void OnDisable()
        {
            ConditionsMet -= OnConditionsMet;
        }

        private static void OnConditionsMet(GameObject player)
        {
            if (player.TryGetComponent(out Health health)) health.Kill();
        }
    }
}