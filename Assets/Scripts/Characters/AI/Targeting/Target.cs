using System;
using UnityEngine;

namespace Characters.Behaviours
{
    /// <summary>
    /// Target for enemies/attacks.
    /// </summary>
    public class Target : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Current target.")]
        private Transform target;
    
        private Transform _targetInternal;

        /// <summary>
        /// Invoked when target is acquired.
        /// </summary>
        public event Action Acquired;

        /// <summary>
        /// Invoked when target is changed.
        /// </summary>
        public event Action Changed;

        /// <summary>
        /// Invoked when target is lost.
        /// </summary>
        public event Action Lost;

        /// <summary>
        /// Current target.
        /// </summary>
        public Transform Get => target;

        /// <summary>
        /// Current target.
        /// </summary>
        public Transform Transform => target;

        /// <summary>
        /// Whether target is set.
        /// </summary>
        public bool HasTarget => target != null;

        private void Awake()
        {
            _targetInternal = target;
        }

        private void OnValidate()
        {
            if (target != _targetInternal) Set(target);
        }

        /// <summary>
        /// Sets new target.
        /// </summary>
        /// <param name="newTarget">New target.</param>
        public void Set(Transform newTarget)
        {
            if (target == null && newTarget != null)
            {
                target = _targetInternal =  newTarget;
                Acquired?.Invoke();
            }
            else if (target != newTarget)
            {
                target = _targetInternal = newTarget;
                (newTarget != null ? Changed : Lost)?.Invoke();
            }
        }

        /// <summary>
        /// Clears target.
        /// </summary>
        public void Clear()
        {
            Set(null);
        }
    }
}