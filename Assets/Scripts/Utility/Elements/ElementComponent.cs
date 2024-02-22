using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elements
{
    /// <summary>
    /// Element of the object.
    /// </summary>
    [DisallowMultipleComponent]
    public class ElementComponent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onAir;
        [SerializeField]
        private UnityEvent onWater;
        [SerializeField]
        private UnityEvent onEarth;
        [SerializeField]
        private UnityEvent onFire;

        [SerializeField]
        [Tooltip("Element of the object.")]
        private Element element = Element.Air;

        /// <summary>
        /// Element has been changed.
        /// </summary>
        public event Action<Element> Changed;

        /// <summary>
        /// <inheritdoc cref="ElementComponent"/>
        /// </summary>
        public Element Current { get; protected set; }

        /// <summary>
        /// Element before current one. Equal if unchanged.
        /// </summary>
        public Element Previous { get; private set; }

        protected virtual void Awake()
        {
            //base.Awake();
            
            Current = Previous = element;
        }

        private void OnValidate()
        {
            // Runtime only
            if (!Application.isPlaying) return;

            // Apply inspector element change.
            //if (element != Current) ChangeElement(element);
        }

        /// <summary>
        /// Change object element.
        /// </summary>
        /// <param name="newElement">Object element after this change.</param>
        /// <returns>Element has been changed.</returns>
        public virtual bool ChangeElement(Element newElement)
        {
            if (newElement == Current) return false;

            Previous = Current;
            Current  = element = newElement;
            Changed?.Invoke(newElement);

            switch (newElement)
            {
                case Element.Air:
                    onAir.Invoke();
                    break;
                case Element.Water:
                    onWater.Invoke();
                    break;
                case Element.Earth:
                    onEarth.Invoke();
                    break;
                case Element.Fire:
                    onFire.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newElement), newElement, null);
            }
            
            return true;
        }
    }
}