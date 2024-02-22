using UnityEngine;

namespace Utility.Miscellaneous
{
    /// <summary>
    /// Rigidbody2D constrainter.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DConstrainter : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void FreezePositionX()
        {
            _rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
        }

        public void FreezePositionY()
        {
            _rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        }

        public void FreezeRotation()
        {
            _rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        }

        public void FreezePosition()
        {
            _rb.constraints |= RigidbodyConstraints2D.FreezePosition;
        }

        public void FreezeAll()
        {
            _rb.constraints |= RigidbodyConstraints2D.FreezeAll;
        }

        public void UnfreezePositionX()
        {
            _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }

        public void UnfreezePositionY()
        {
            _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }

        public void UnfreezeRotation()
        {
            _rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        }

        public void UnfreezePosition()
        {
            _rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        }

        public void UnfreezeAll()
        {
            _rb.constraints &= ~RigidbodyConstraints2D.FreezeAll;
        }
    }
}
