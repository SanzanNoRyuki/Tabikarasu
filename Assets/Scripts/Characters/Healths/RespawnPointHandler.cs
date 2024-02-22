using UnityEngine;

namespace Characters.Healths
{
    /// <summary>
    /// Character respawn point handler.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Health))]
    public class RespawnPointHandler : MonoBehaviour
    {
        private Health _health;

        private Vector2 _spawnPoint;

        /// <summary>
        /// <br/>Point at which object will be respawned.
        /// <br/>Spawn point by default.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Point at which object will be respawned.\nSpawn point by default.")]
        public Vector2 RespawnPoint { get; private set; }

        private void Awake()
        {
            _health = GetComponent<Health>();

            RespawnPoint = _spawnPoint = transform.position;
        }

        private void OnEnable()
        {
            _health.Respawned += TeleportToRespawnPoint;
        }

        private void OnDisable()
        {
            _health.Respawned -= TeleportToRespawnPoint;
        }

        /// <summary>
        /// <inheritdoc cref="SetRespawnPoint(UnityEngine.Vector2)"/>
        /// </summary>
        [ContextMenu("Set respawn point at current position.")]
        public void SetRespawnPoint()
        {
            SetRespawnPoint(transform.position);
        }

        /// <summary>
        /// Set respawn point to position. Current position is used by default.
        /// </summary>
        /// <param name="position">New respawn point position.</param>
        public void SetRespawnPoint(Vector2 position)
        {
            RespawnPoint = position;
        }

        /// <summary>
        /// Reset respawn point to default position.
        /// </summary>
        [ContextMenu("Reset respawn point.")]
        public void ResetRespawnPoint()
        {
            SetRespawnPoint(_spawnPoint);
        }

        /// <summary>
        /// Teleport to spawn point.
        /// </summary>
        [ContextMenu("Teleport to respawn point.")]
        public void TeleportToRespawnPoint()
        {
            transform.position = RespawnPoint;
        }
    }
}