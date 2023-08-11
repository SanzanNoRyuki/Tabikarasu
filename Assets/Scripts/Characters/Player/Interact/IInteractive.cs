using UnityEngine;

namespace Attacks.Affectables
{
    public interface IInteractive
    {
        public bool Interact(Rigidbody2D interactor);

        /// <summary>
        /// Optional release interaction.
        /// </summary>
        public void InteractRelease(Rigidbody2D interactor) { }
    }
}
