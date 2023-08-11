using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Attributes;
using Utility.Constants;

namespace Utility.Physics_Checks
{
    /// <summary>
    /// Checks if object is grounded.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class GroundCheck : MonoBehaviour
    {
        // Helper collection.
        // There are usually only 1-2 contacts during collision (though possible range is 1 to infinity).
        private static readonly List<ContactPoint2D> Contacts = new(capacity: 2);
        
        private Rigidbody2D _rb;
        
        /// <summary>
        /// Object is touching ground.
        /// </summary>
        [field: ReadOnly]
        [field: SerializeField]
        [field: Tooltip("Object is touching ground.")]
        public bool Grounded { get; private set; }
        
        /// <summary>
        /// What is ground?
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("What is ground?")]
        public LayerMask Ground { get; private set; } = Physics2D.AllLayers;
        private ContactFilter2D _ground;

        [field: ReadOnly]
        [field: SerializeField]
        public Collider2D GroundCollider { get; private set; }

        /// <summary>
        /// Raised when object touches the ground.
        /// </summary>
        public event Action Landed;

        /// <summary>
        /// Raised when object leaves the ground.
        /// </summary>
        public event Action TakenOff;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            ResetProperties();
        }

        private void Start()
        {
            UpdateGroundedStatus();
        }

        private void Update()
        {
            UpdateGroundedStatus();
        }

        private void OnValidate()
        {
            ResetProperties();
        }

        protected virtual void ResetProperties()
        {
            _ground.SetLayerMask(Ground);
        }
        
        private void UpdateGroundedStatus()
        {
            // Reuse helper collection.
            Contacts.Clear();
            _rb.GetContacts(_ground, Contacts);

            GroundCollider = null;
            bool grounded = false;
            foreach (ContactPoint2D contact in Contacts.Where(contact => contact.normal.y >= Values.Sin45))
            {
                grounded       = true;
                GroundCollider = contact.collider;
                break;
            }

            if (Grounded != grounded)
            {
                Grounded = grounded;
                (grounded ? Landed : TakenOff)?.Invoke();
            }
        }
    }
}