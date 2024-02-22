using Characters.Healths;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Environment.Zones
{
    /// <summary>
    /// Out of bounds killer.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Void : MonoBehaviour
    {
        protected void OnTriggerExit2D(Collider2D otherCollider)
        {
            GameObject other = otherCollider.Other();

            if (other.CompareTag(Tags.Player) && other.TryGetComponent(out Health player))
            {
                // Don't kill player if it's a trigger only yet.
                if (otherCollider.isTrigger) return;
                if (player.IsAlive) player.Kill();
            }
            else
            {
                if (other.activeInHierarchy) other.SetActive(false);
            }
        }
    }
}


