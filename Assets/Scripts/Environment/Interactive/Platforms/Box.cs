using Attacks.Affectables;
using Characters.Player.Movement;
using UnityEngine;
using Utility.Physics_Checks;

namespace Environment.Interactive.Springs.Platforms
{
    /// <summary>
    /// Movable crate box.
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Joint2D))]
    public class Box : MonoBehaviour, IInteractive
    {
        private Joint2D joint;

        private void Awake()
        {
            if (transform.parent != null) Debug.LogWarning("Box object cannot have parent at the start.");

            joint = GetComponent<Joint2D>();

            joint.enabled = false;
        }

        public bool Interact(Rigidbody2D interactor)
        {
            if (joint.enabled) return false;
            if (joint.connectedBody != null) return false;

            if (interactor.TryGetComponent(out GroundCheck groundCheck) && !groundCheck.Grounded) return false;
            if (interactor.transform.position.y > transform.position.y) return false;

            joint.enabled          =  true;
            joint.connectedBody    =  interactor;
            //interactor.constraints |= RigidbodyConstraints2D.FreezePositionY;

            if (interactor.TryGetComponent(out PlayerMovement movement))
            {
                movement.IsPulling = true;
            }

            return true;
        }

        public void InteractRelease(Rigidbody2D interactor)
        {
            if (joint.connectedBody != interactor) return;

            joint.connectedBody    =  null;
            joint.enabled          =  false;
            interactor.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

            if (interactor.TryGetComponent(out PlayerMovement movement))
            {
                movement.IsPulling = false;
                movement.ResetJumpTokens();
            }
        }
    }
}