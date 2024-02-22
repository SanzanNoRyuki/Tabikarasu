using System;
using UnityEngine;

namespace Attacks.Charges
{
    /// <summary>
    /// Charge adding various effects to the attack or object.
    /// </summary>
    [RequireComponent(typeof(Attack))]
    public abstract class Charge : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Attack>().OnHit += OnHit;
        }

        private void OnDisable()
        {
            GetComponent<Attack>().OnHit -= OnHit;
        }
        
        /// <summary>
        /// Foo.
        /// </summary>
        /// <param name="target">a.</param>
        /// <param name="direction"></param>
        protected abstract void OnHit(GameObject target, Vector2 direction);
    }
}