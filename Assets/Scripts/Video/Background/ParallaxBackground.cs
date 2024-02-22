using UnityEngine;

namespace Video.Background 
{
    /// <summary>
    /// Parallax effect background.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Parallax effect on X axis.")]
        private float parallaxX = 0.5f;

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Parallax effect on Y axis.")]
        private float parallaxY;
    
        private Transform _camera;
        private Vector3   _lastCameraPosition;
        private float     _textureUnitSizeX;

        private Vector2 Parallax => new(parallaxX, parallaxY);

        private void Start()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Sprite         sprite         = spriteRenderer.sprite;
            Texture2D      texture        = sprite.texture;
            _textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.lossyScale.x;
        }

        private void OnEnable()
        {
            if (UnityEngine.Camera.main != null)
            {
                _camera = UnityEngine.Camera.main.transform;
            }
            else
            {
                Debug.LogError("No camera found!", this);
                enabled = false;
                return;
            }

            _lastCameraPosition = _camera.position;
        }
        
        private void LateUpdate()
        {
            Vector3 cameraPosition = _camera.position;
            Vector3 cameraDelta    = cameraPosition - _lastCameraPosition;

            Transform tf = transform;

            tf.position  -= new Vector3(cameraDelta.x * Parallax.x, cameraDelta.y * Parallax.y);
            _lastCameraPosition =  cameraPosition;

            if (Mathf.Abs(cameraPosition.x - tf.position.x) >= _textureUnitSizeX)
            {
                Vector3 currentPosition = tf.position;
                float   offsetPositionX = (cameraPosition.x - currentPosition.x) % _textureUnitSizeX;
                currentPosition    = new Vector3(cameraPosition.x + offsetPositionX, currentPosition.y);
                tf.position = currentPosition;
            }
        }
    }
}