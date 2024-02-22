using UnityEngine;
using Utility.Attributes;

namespace Attacks.Projectiles
{
    /// <summary>
    /// Object that follows another object until its destruction/disabling. It then stays where it was until its respawned. It attaches to a parent.
    /// </summary>
    [DisallowMultipleComponent]
    public class ProjectileTrailer : MonoBehaviour
    {
        /// <summary>
        /// Object being followed.
        /// </summary>
        [field: ReadOnly]
        [field: SerializeField]
        [field: Tooltip("Object being followed.")]
        public Transform Trailed { get; private set; }

        /// <summary>
        /// Is object currently trailing its <see cref="Trailed">trailed</see> object?
        /// </summary>
        public bool IsTrailing { get; private set; }


        private Quaternion _originalRotation;

        private void Awake()
        {
            // Store object to follow.
            Trailed = transform.parent;
        
            // Detach from parent.
            transform.SetParent(null, true);
        }

        private void LateUpdate()
        {
            // Trailed object doesn't exist anymore.
            if (Trailed == null)
            {
                enabled = false;
                return;
            }


            Quaternion newRotation = Quaternion.Euler(Trailed.rotation.eulerAngles.x, Trailed.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);


            switch (IsTrailing)
            {
                // Trailed object was disabled.
                case true when !Trailed.gameObject.activeInHierarchy:
                    IsTrailing = false;
                    if (TryGetComponent(out ParticleSystem particles)) particles.Stop();
                    break;
                // Trailing object.
                case true:
                    transform.SetPositionAndRotation(Trailed.position, newRotation);
                    break;
                // Trailed object was re-enabled.
                case false when Trailed.gameObject.activeInHierarchy:
                {
                    IsTrailing         = true;
                    transform.SetPositionAndRotation(Trailed.position, newRotation);

                    if (TryGetComponent(out TrailRenderer trail)) trail.Clear();

                    if (TryGetComponent(out ParticleSystem particlesg))
                    {
                        particlesg.Clear();
                        particlesg.Play();
                    }

                    break;
                }
            }
        }

    }
}