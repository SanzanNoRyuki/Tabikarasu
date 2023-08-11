using System;
using System.Collections;
using UnityEngine;
using Utility.Attributes;
using Utility.Extensions;

namespace Utility.Physics_Checks
{
    /// <summary>
    /// Checks if object faces wall.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class  WallCheck : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        [Tooltip("Object is facing and touching wall.")]
        private bool walled;

        [Fixed]
        [SerializeField]
        [Tooltip("What is wall?")]
        private LayerMask walls = Physics2D.AllLayers;

        private Rigidbody2D     _rb;
        private ContactFilter2D _wallFilter;

        /// <summary>
        /// Raised when object touches the wall.
        /// </summary>
        public event Action GotWalled;

        /// <summary>
        /// Raised when object stops facing wall.
        /// </summary>
        public event Action StoppedFacingWall;

        /// <summary>
        /// Object is facing and touching wall.
        /// </summary>
        public bool Walled => walled;

        /// <summary>
        /// What is wall?
        /// </summary>
        public LayerMask Walls => walls;

        [Range(0f, 0.5f)]
        [SerializeField]
        private float dizzyTime = 0.1f;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!walls.Contains(collision.gameObject.layer))         return;

            if (walled)
            {
                if (transform.FacingRight2D() && !collision.FromRight())
                {
                    walled = false;
                    StoppedFacingWall?.Invoke();
                    return;
                }
                if (transform.FacingLeft2D()  && !collision.FromLeft())
                {
                    walled = false;
                    StoppedFacingWall?.Invoke();
                    return;
                }

                
            }
            else
            {
                if (transform.FacingLeft2D()  && !collision.FromLeft()) return;
                if (transform.FacingRight2D() && !collision.FromRight())
                {
                    return;
                }

                if (transform.FacingRight2D() && collision.FromRight())
                {
                    walled = true;
                    GotWalled?.Invoke();
                    return;
                }

                if (transform.FacingLeft2D() && collision.FromLeft())
                {
                    walled = true;
                    GotWalled?.Invoke();
                    return;
                }
            }
        }

        private void CheckWalls(Collision2D collision)
        {
            
        }

        private void CheckFree(Collision2D collision)
        {
            
        }


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _wallFilter.SetLayerMask(walls);
        }
        /*
        private void FixedUpdate()
        {
            //if (_landed) return;


            RaycastHit2D[] hits = new RaycastHit2D[1];
            int hitCount = _rb.Cast(transform.right, _wallFilter, hits, 0.1f);

            bool status = hitCount > 0;

            if (status == walled) return;

            if (status)
            {
                walled = true;
                GotWalled?.Invoke();
            }
            else
            {
                walled = false;
                StoppedFacingWall?.Invoke();
            }

            
            if (walled)
            {
                if (transform.FacingRight2D() && _rb.HasContactFromRight(_wallFilter)) return;
                if (transform.FacingLeft2D()  && _rb.HasContactFromLeft(_wallFilter)) return;

                walled = false;
                StoppedFacingWall?.Invoke();
                StartCoroutine(LadingWaiter());
            }
            else
            {
                if (transform.FacingRight2D()    && !_rb.HasContactFromRight(_wallFilter))return;
                if (transform.FacingLeft2D() && !_rb.HasContactFromLeft(_wallFilter)) return;

                walled = true;
                GotWalled?.Invoke();
                StartCoroutine(LadingWaiter());
            }
            
        }*/

        private void OnValidate()
        {
            if (Application.isPlaying) _wallFilter.SetLayerMask(walls);
        }
    }
}