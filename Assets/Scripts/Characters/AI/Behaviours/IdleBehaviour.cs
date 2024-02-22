using System.Collections;
using System.Collections.Generic;
using Audio;
using Characters.Behaviour;
using Unity.VisualScripting;
using UnityEngine;
using Utility.Extensions;

namespace Characters.Behaviours
{

    /// <summary>
    /// By itself, does nothing.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class IdleBehaviour : EntityBehaviour
    {
        [Range(0f, 10f)]
        [SerializeField]
        [Tooltip("Drag applied to the rigidbody when idle. Used to slowdown character.")]
        private float idleDrag = 0.5f;
        private float _previousDrag;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _previousDrag = _rb.drag;
            _rb.drag = idleDrag;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _rb.drag = _previousDrag;
        }
    }
}