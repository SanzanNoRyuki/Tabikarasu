using System;
using UnityEngine;

namespace Utility.Miscellaneous
{
    /// <summary>
    /// Sprite mask that follows the sprite renderer.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(SpriteMask))]
    [DisallowMultipleComponent]
    public class SpriteRendererMask : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private SpriteMask     _spriteMask;

        private void OnValidate()
        {
            GetComponent<SpriteMask>().sprite = GetComponent<SpriteRenderer>().sprite;

            
        }

        private void OnEnable()
        {
            if (!TryGetComponent(out Animator _)) enabled = false;
        }

        private void Awake()
        {
            _spriteMask     = GetComponent<SpriteMask>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _spriteMask.sprite = _spriteRenderer.sprite;
        }
    }
}