using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utility.Attributes;

namespace Attacks.Affectables
{
    /// <summary>
    /// Object capable of getting petrified.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Petrifiable : MonoBehaviour
    {
        /// <summary>
        /// Is this object is currently petrified?
        /// </summary>
        [field: ReadOnly]
        [field: SerializeField]
        public bool IsPetrified { get; private set; }

        [SerializeField]
        [Tooltip("Associated sprite renderer.")]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        [Tooltip("Object got petrified.")]
        private UnityEvent petrified;

        [SerializeField]
        [Tooltip("Object got unpetrified.")]
        private UnityEvent unpetrified;

        /// <summary>
        /// Object got petrified.
        /// </summary>
        public event Action Petrified;

        /// <summary>
        /// Object got unpetrified.
        /// </summary>
        public event Action Unpetrified;

        private Rigidbody2D                  _rb;
        private Animator                     _animator;
        private IPetrificationShaderInstance _shaderInstance;
        private RigidbodyConstraints2D       _constraints;
        private Coroutine                    _petrifyingCoroutine;

        private void Awake()
        {
            _rb          = GetComponent<Rigidbody2D>();
            _constraints = _rb.constraints;

            if (spriteRenderer == null || !spriteRenderer.TryGetComponent(out _shaderInstance))
            {
                Debug.LogError($"Petrifiable requires {nameof(IPetrificationShaderInstance)} on associated {nameof(SpriteRenderer)}.", this);
                enabled = false;
            }
            else
            {
                spriteRenderer.TryGetComponent(out _animator);
            }
        }

        private void OnDisable()
        {
            if (IsPetrified) Unpetrify();
        }

        [ContextMenu("Petrify")]
        public void Petrify()
        {
            Petrify(5f);
        }

        /// <summary>
        /// Petrify this object.
        /// </summary>
        public bool Petrify(float duration)
        {
            if (IsPetrified) return false;

            if (_petrifyingCoroutine != null) StopCoroutine(_petrifyingCoroutine);
            _petrifyingCoroutine = StartCoroutine(Petrifying(duration));
            return true;
        }

        /// <summary>
        /// Unpetrify this object.
        /// </summary>
        public bool Unpetrify()
        {
            if (!IsPetrified) return false;
            
            if (_petrifyingCoroutine != null) StopCoroutine(_petrifyingCoroutine);
            if (_animator            != null) _animator.enabled = true;
            _rb.constraints = _constraints;
            _shaderInstance.Unpetrify();

            IsPetrified = false;
            unpetrified.Invoke();
            Unpetrified?.Invoke();
            return true;
        }

        private IEnumerator Petrifying(float duration)
        {
            Debug.Log(duration);
            
            IsPetrified = true;

            duration = Mathf.Max(0f, duration);

            float shadingTime = 0.2f * duration;
            float frozenTime  = 0.8f * duration;

            if (_animator != null) _animator.enabled = false;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _shaderInstance.Petrify(shadingTime);
            
            petrified.Invoke();
            Petrified?.Invoke();


            if (frozenTime > 0f) yield return new WaitForSeconds(frozenTime);
            _shaderInstance.Unpetrify(shadingTime);
            
            yield return new WaitForSeconds(shadingTime);

            Unpetrify();
        }
    }
}
