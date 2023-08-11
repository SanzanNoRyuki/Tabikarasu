using Characters.Player.Movement;
using UnityEngine;

namespace Characters.Player.Skills.Elemental.Passives
{
    /// <summary>
    /// Allows player to <see cref="PlayerMovement.PhaseDash">phase</see> through certain objects.
    /// </summary>
    public class PhaseDash : Passive
    {
        [SerializeField]
        [Tooltip("Player movement component.")]
        private PlayerMovement movement;

        private void OnEnable()
        {
            if (movement == null && (movement = GetComponentInParent<PlayerMovement>()) == null)
            {
                Debug.LogWarning($"Required component {nameof(PlayerMovement)} not assigned. Passive is disabled.", this);
                enabled = false;
            }
            else
            {
                movement.AllowPhaseDash();
            }
        }

        private void OnDisable()
        {
            movement.DisallowPhaseDash();
        }
    }
}