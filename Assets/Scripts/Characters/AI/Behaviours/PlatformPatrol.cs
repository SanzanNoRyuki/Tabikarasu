using System;
using System.Collections;
using Audio;
using Miscellaneous.Movement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utility.Attributes;
using Utility.Extensions;
using Utility.Physics_Checks;

namespace Characters.Behaviour
{

    /// <summary>
    /// Patrols on one platform.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EdgeCheck))]
    [RequireComponent(typeof(WallCheck))]
    public class PlatformPatrol : EntityBehaviour
    {
        public const float MinSpeed = 0f;
        public const float MaxSpeed = 100f;

        public const float MinAcceleration = 0.01f;
        public const float MaxAcceleration = 10f;
        
        [SerializeField]
        private float wallDelay = 0f;

        [SerializeField]
        private float ledgeDelay = 0.5f;

        [SerializeField]
        private UnityEvent stopped;
        [SerializeField]
        private UnityEvent moved;


        [SerializeField]
        [Range(MinSpeed, MaxSpeed)]
        private float speed = 14f;

        [ReadOnly]
        [SerializeField]
        [Tooltip("Speed modifier used by other scripts.")]
        private float speedModifier = 1f;

        [SerializeField]
        [Range(MinAcceleration, MaxAcceleration)]
        private float acceleration = 10f;

        private Rigidbody2D _rb;
        private EdgeCheck   _edgeCheck;
        private WallCheck   _wallCheck;

        private void Awake()
        {
            _rb        = GetComponent<Rigidbody2D>();
            _edgeCheck = GetComponent<EdgeCheck>();
            _wallCheck = GetComponent<WallCheck>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _wallCheck.GotWalled  += OnWalled;
            _edgeCheck.GotToLedge += OnLedge;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _wallCheck.GotWalled  -= OnWalled;
            _edgeCheck.GotToLedge -= OnLedge;

            _rb.velocity = new Vector2(0f, _rb.velocity.y);
        }


        protected virtual void OnWalled()
        {
            StopAndFlip(wallDelay);
        }

        protected virtual void OnLedge()
        {
            StopAndFlip(ledgeDelay);
        }


        private void FixedUpdate()
        {
            Move();
        }

        protected virtual void StopAndFlip(float delay)
        {
            if (dizzy) return;
            StartCoroutine(WaitAndFlip(delay));
        }

        protected void Flip()
        {
            StartCoroutine(WaitAndFlip(wallDelay));
        }

        protected void Stop()
        {
            _rb.velocity = new Vector2(0f, _rb.velocity.y);
        }

        private bool idle;
        private bool dizzy;
        
        private IEnumerator WaitAndFlip(float seconds)
        {
            _rb.velocity = new Vector2(0f, _rb.velocity.y);

            idle  = true;
            dizzy = true;
            stopped.Invoke();
            yield return new WaitForSeconds(seconds);

            transform.FlipX();
            idle = false;

            yield return new WaitForSeconds(dizzyTime);
            dizzy = false;
            moved.Invoke();
        }

        [Range(0f, 0.5f)]
        [SerializeField]
        private float dizzyTime = 0.1f;

        protected virtual void Move()
        {
            IsMoving = false;
            if (!CanMove || idle) return;
            IsMoving = true;
            
            var direction       = transform.right.x;
            var speedDifference = direction * speedModifier * speed - _rb.velocity.x;
            var force           =  _rb.mass * acceleration * speedDifference;
            

            _rb.AddForce(Vector2.right * force);
        }

        public bool IsMoving
        {
            get; private set;

        }


        public bool canMove = true;
        public bool CanMove => canMove && (_rb.constraints & RigidbodyConstraints2D.FreezePositionX) == 0;
    }
} 