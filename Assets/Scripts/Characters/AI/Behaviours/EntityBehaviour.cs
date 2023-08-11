using System;
using System.Collections.Generic;
using Characters.Behaviours;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utility.Attributes;

namespace Characters.Behaviour
{
    /// <summary>
    /// Non-playable entity brain consisting from state machine.
    /// </summary>
    /// <remarks>
    /// Multiple behaviours require brain to be able to switch between them.
    /// </remarks>  
    public abstract class EntityBehaviour : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        [Tooltip("Name of the state behaviour is used in.")]
        // ReSharper disable once NotAccessedField.Local -- Used for debugging purposes.
        private string stateName;

        [SerializeField]
        private UnityEvent gotEnabled;
        public event Action GotEnabled;
        
        [SerializeField]
        private UnityEvent gotDisabled;
        public event Action GotDisabled;

        protected virtual void OnEnable()
        {
            EntityBehaviour[] behaviours = GetComponents<EntityBehaviour>();
            foreach (EntityBehaviour behaviour in behaviours)
            {
                if (behaviour == this) continue;
                behaviour.enabled = false;
            }

            gotEnabled.Invoke();
            GotEnabled?.Invoke();
        }

        protected virtual void OnDisable()
        {
            gotDisabled.Invoke();
            GotDisabled?.Invoke();
        }
    }
}