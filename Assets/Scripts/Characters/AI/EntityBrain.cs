using System.Collections.Generic;
using Characters.Behaviour;
using UnityEngine;
using Utility.Attributes;

namespace Characters.Behaviours
{
    /// <summary>
    /// Non-playable entity brain consisting from state machine.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Target))]
    public class EntityBrain : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        [Tooltip("Name of the current state.")]
        // ReSharper disable once NotAccessedField.Local -- Used for debugging purposes.
        private string currentState;
        
        private EntityBehaviour _currentBehaviour;

        [SerializeField]
        private EntityBehaviour defaultBehaviour;
        
        [SerializeField]
        private List<EntityState> states = new();

        private Target _target;
        
        private void Awake()
        {
            _target = GetComponent<Target>();

            // Sort states by range.
            states.Sort((a, b) => a.Range.CompareTo(b.Range));
        }

        private void OnValidate()
        {
            states.Sort((a, b) => a.Range.CompareTo(b.Range));

        }

        private void OnEnable()
        {
            if (defaultBehaviour == null)
            {
                if (states.Count > 0)
                {
                    defaultBehaviour = states[0].Behaviour;
                }
                else
                {
                    Debug.LogWarning("Entity brain has no default behaviour and no states. Disabling brain.");
                    enabled = false;
                    return;
                }
            }
            else
            {
                if (defaultBehaviour.gameObject != gameObject)
                {
                    Debug.LogWarning("Default behaviour is not part of this gameobject. Disabling brain.");
                    enabled = false;
                    return;
                }
            }

            foreach (EntityState state in states)
            {
                if (state.Behaviour == null)
                {
                    Debug.LogWarning("Entity brain has a state with no behaviour. Disabling brain.");
                    enabled = false;
                    return;
                }

                if (state.Behaviour.gameObject != gameObject)
                {
                    Debug.LogWarning("State behaviour is not part of this gameobject. Disabling brain.");
                    enabled = false;
                    return;
                }
            }

            _currentBehaviour = defaultBehaviour;
            _currentBehaviour.enabled = true;
        }

        

        private void Update()
        {
            float distance = _target.HasTarget ? Vector2.Distance(transform.position, _target.Transform.position) : float.PositiveInfinity;

            


            foreach (EntityState state in states)
            {
                // Skip states that are out of range.
                if (state.Range     < distance) continue;

                // If state is already active, exit.
                if (state.Behaviour == _currentBehaviour) return;

                // Disable current behaviour and enable new one.
                state.Behaviour.enabled = true;
                _currentBehaviour = state.Behaviour;
                currentState = state.Behaviour.name;
                return;
            }

            // If no state is active, enable default behaviour.
            defaultBehaviour.enabled = true;
            _currentBehaviour        = defaultBehaviour;
            currentState    = "Default";
        }

        [System.Serializable]
        private class EntityState
        {
            [SerializeField]
            [Tooltip("Name of the state. Used for debugging purposes.")]
            // ReSharper disable once NotAccessedField.Local
            private string name;

            [field: SerializeField]
            public float Range { get; private set; } = 1f;

            [field: SerializeField]
            public EntityBehaviour Behaviour { get; private set; }
        }
    }
}