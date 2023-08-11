using UnityEngine;
using Utility.Constants;

namespace Video.Background
{
    /// <summary>
    /// Automatically scrolling background.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class AutoScrollBackground : MonoBehaviour
    {
        [Range(0f, 10f)]
        [SerializeField]
        [Tooltip("Speed of scrolling.")]
        private float speed = 0.5f;
        
        [SerializeField]
        [Tooltip("Direction of scrolling.")]
        private Direction direction = Direction.Left;

        private Transform _root;
        private float     _textureWidth;
        
        private void Start()
        {
            _root         = transform.root;
            _textureWidth = CalculateTextureWidth();
        }

        private void OnValidate()
        {
            _textureWidth = CalculateTextureWidth();
        }

        private void Update()
        {
            Transform tf                = transform;
            Vector3   transformPosition = tf.position;

            switch (direction)
            {
                case Direction.Up:
                    transformPosition.y += speed * Time.deltaTime;
                    break;
                case Direction.Down:
                    transformPosition.y -= speed * Time.deltaTime;
                    break;
                case Direction.Left:
                    transformPosition.x -= speed * Time.deltaTime;
                    break;
                case Direction.Right:
                    transformPosition.x += speed * Time.deltaTime;
                    break;
                default:
                    throw new System.NotImplementedException();
            }

            tf.position = transformPosition;

            Vector3 rootPosition = _root.position;

            if (Mathf.Abs(rootPosition.x - transformPosition.x) >= _textureWidth)
            {
                float offsetPositionX = (rootPosition.x - transformPosition.x) % _textureWidth;
                tf.position = new Vector3(rootPosition.x + offsetPositionX, transformPosition.y);
            }
        }

        private float CalculateTextureWidth()
        {
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            return sprite.texture.width / sprite.pixelsPerUnit * transform.lossyScale.x;
        }
    }
}