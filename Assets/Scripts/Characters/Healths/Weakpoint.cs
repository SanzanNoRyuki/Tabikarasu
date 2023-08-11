using UnityEngine;
using Utility.Extensions;

namespace Characters.Healths
{
    /// <summary>
    /// Spot that inflicts damage to assigned health if touched.
    /// </summary>
    [DisallowMultipleComponent]
    public class Weakpoint : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Layers that can hurt this weak spot.")]
        private LayerMask weakTo = Physics2D.AllLayers;

        [SerializeField]
        [Range(Health.MinDamage, Health.MaxDamage)]
        [Tooltip("Damage that this weakpoint will inflict to assigned health.")]
        private int damage;

        [SerializeField]
        [Tooltip("Affected health component.")]
        private Health health;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (health == null)
            {
                health = GetComponentInParent<Health>();

                if (health == null)
                {
                    Debug.LogError($"{nameof(Weakpoint)} requires {nameof(Health)} component to be assigned.", this);
                    enabled = false;
                    return;
                }
            }

            if (collision.rigidbody == null) return;
            if (!weakTo.Contains(collision.rigidbody.gameObject.layer)) return;

            health.Damage(damage);
        }
    }
}