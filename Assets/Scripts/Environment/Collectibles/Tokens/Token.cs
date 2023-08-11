using System;
using Environment.Interactive.Doors;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Utility.Constants;
using Utility.Extensions;

namespace Environment.Collectibles.Tokens    
{
    /// <summary>
    /// Collectible token.
    /// </summary>
    [SelectionBase]
    public abstract class Token : ProgressCondition
    {
        [SerializeField]
        [Range(1, 1000)]
        [Tooltip("Value of the token.")]
        private int value = 1;

        [SerializeField]
        [Tooltip("Token got collected.")]
        private UnityEvent<GameObject> collected;

        /// <summary>
        /// Token got collected.
        /// </summary>
        public event Action<GameObject> Collected;
        
        private void OnColliderEnter2D(Collision2D collision)
        {
            if (!collision.collider.isTrigger && collision.Other().CompareTag(Tags.Player)) Collect();
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (!otherCollider.isTrigger && otherCollider.Other().CompareTag(Tags.Player)) Collect();
        }

        public abstract void Add(int value);

        [ContextMenu("Add 1")]
        public void Add()
        {
            Add(1);
        }

        [ContextMenu("Collect")]
        public void Collect()
        {
            Collect(null);
        }

        public void Collect([CanBeNull] GameObject collector)
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Token collection disabled in editor to prevent accidental collection.", this);
                return;
            }
            #endif

            Add(value);

            collected.Invoke(collector);
            Collected?.Invoke(collector);

            gameObject.SetActive(false);
        }

        public override bool ConditionMet()
        {
            return !gameObject.activeSelf;
        }
    }
}
