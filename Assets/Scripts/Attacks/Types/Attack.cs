using System;
using UnityEngine;
using Utility.Extensions;

namespace Attacks
{
    /// <summary>
    /// Attack capable of hitting <see cref="Targets">targets</see> .
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Attack : MonoBehaviour
    {
        [SerializeField]
        private bool ignoreTriggers = true;

        /// <summary>
        /// Targets that can result in <see cref="OnHit"/> event.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Targets that can result in hit event.")]
        public LayerMask Targets { get; protected set; } = Physics2D.AllLayers;

        /// <summary>
        /// <br/>Action called on valid target hit.
        /// <br/> 1st parameter: TargetOld rigidbody.
        /// <br/> 2nd parameter: Direction of the hit.
        /// </summary>
        public abstract event Action<GameObject, Vector2> OnHit;

        /// <summary>
        /// TargetOld can be hit by this attack.
        /// </summary>
        /// <param name="target">TargetOld to validate.</param>
        /// <returns>True if target belongs to <see cref="Targets"/>.</returns>
        public bool IsValidTarget(Collider2D target)
        {
            return (!ignoreTriggers || !target.isTrigger) && Targets.Contains(target.attachedRigidbody != null ? target.attachedRigidbody.gameObject.layer : target.gameObject.layer);
        }

        public bool IsValidTarget(Collider2D target, out GameObject go)
        {
            go = target.attachedRigidbody != null ? target.attachedRigidbody.gameObject : target.gameObject;

            return (!ignoreTriggers || !target.isTrigger) && Targets.Contains(go.layer);
        }
    }
}