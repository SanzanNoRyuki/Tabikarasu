using UnityEngine;

namespace Video.Particle_Systems
{
    public class FixedPositionParticleSystem : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private Vector3        _localPosition;
        private Vector3        _offset;

        private bool _wasEmmiting;

        private Vector3 _stayPosition;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _localPosition  = transform.localPosition;
        }

        private void LateUpdate()
        {
            Transform tf = transform;
            
            if (_particleSystem.isPlaying)
            {
                UpdateStayPosition();
                _wasEmmiting = true;
                tf.position  = _stayPosition;
            }
            else
            {
                _wasEmmiting = false;
                tf.position  = tf.parent.position + _localPosition;
            }
        }

        private void UpdateStayPosition()
        {
            if (!_wasEmmiting) _stayPosition = transform.position;
        }
    }
}