using Characters.Player.Movement;
using UnityEngine;

namespace Characters.Player.Skills.Elemental.Passives
{
    /// <summary>
    /// Allows player to jump multiple times.
    /// </summary>
    public class MultiJump : Passive
    {
        [SerializeField]
        [Range(PlayerMovement.MinExtraJumps, PlayerMovement.MaxExtraJumps)]
        [Tooltip("Extra jumps allowed.")]
        private int extraJumps = 1;

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
                movement.AddExtraJumps(extraJumps);
            }
        }

        private void OnDisable()
        {
            movement.RemoveExtraJumps(extraJumps);
        }
    }
}