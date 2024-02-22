using UnityEngine;
using Utility.Extensions;

namespace Environment.Interactive.Platforms
{
    /// <summary>
    /// <br/>Makes colliding objects move together with this platform.
    /// <br/>Objects with parents are ignored to prevent their loss.
    /// <br/>Objects without dynamic <see cref="Rigidbody2D"/> are ignored.
    /// </summary>
    [DisallowMultipleComponent]
    public class StickyPlatform : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Layers that can stick to this platform.")]
        private LayerMask canStick = Physics2D.AllLayers;
        
        [SerializeField]
        [Tooltip("Allow attachments from the top.")]
        private bool stickyTop = true;
        
        [SerializeField]
        [Tooltip("Allow attachments from the side.")]
        private bool stickySides;

        [SerializeField]
        [Tooltip("Allow attachments from the bottom.")]
        private bool stickyBottom;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.rigidbody == null) return;

            Transform other = collision.Other().transform;
            
            if (other.parent != null)                                    return;
            if (collision.rigidbody.bodyType != RigidbodyType2D.Dynamic) return;
            if (!canStick.Contains(other.gameObject.layer))              return;

            if      (stickyTop    && collision.FromLocalTop())    other.SetParent(transform, true);
            else if (stickySides  && collision.FromLocalSide())   other.SetParent(transform, true);
            else if (stickyBottom && collision.FromLocalBottom()) other.SetParent(transform, true);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.rigidbody == null) return;
            
            Transform other = collision.rigidbody.transform;

            if (!other.IsChildOf(transform)) return;
            
            other.SetParent(null, true);
        }
    }
}
