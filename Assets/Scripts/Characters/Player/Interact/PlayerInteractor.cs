using System;
using System.Collections.Generic;
using System.Linq;
using Attacks.Affectables;
using Input_System;
using Input_System.Game_Controller;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility.Attributes;
using Utility.Extensions;

namespace Characters.Behaviours
{
    /// <summary>
    /// Player interaction module.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Layers that can be interacted with. Order is important. Lower priority layers will be interacted with first.")]
        private List<LayerMask> interactive;
        private ContactFilter2D _interactive;
        
        [SerializeField]
        [Tooltip("Collider used for interaction detection.")]
        private Collider2D interactionCollider;

        private IInteractive _current;

        private List<Collider2D> _potentialTargets = new();

        private Rigidbody2D _rb;
        
        protected void Awake()
        {
            _interactive = new ContactFilter2D
            {
                useTriggers  = true,
                useLayerMask = true,
                layerMask    = interactive.Aggregate(0, (current, layer) => current | layer.value),
            };

            _rb = transform.GetComponentInParent<Rigidbody2D>();
        }

        private void OnValidate()
        {
            // Ensure only one layer is selected per mask
            for (int i = 0; i < interactive.Count; i++) interactive[i] = interactive[i].Unique();

            if (!Application.isPlaying) return;

            // Ensure priority order is unique
            interactive = interactive.Distinct().ToList();
        }


        private void OnEnable()
        {
            if (interactionCollider == null)
            {
                enabled = false;
                Debug.LogError("Interaction collider is not set.", this);
            }


            GameController.Subscribe(GameController.Actions.Interact, Interact);
        }

        private void Update()
        {
            if (_current != null) transform.rotation = _rotation;
        }

        private Quaternion _rotation;

        private void OnDisable()
        {
            GameController.Unsubscribe(GameController.Actions.Interact, Interact);
        }

        [ContextMenu("Interact")]
        public void Interact()
        {
            // Reuse collider list
            _potentialTargets.Clear();
            interactionCollider.OverlapCollider(_interactive, _potentialTargets);

            // Sort colliders by priority
            _potentialTargets = _potentialTargets.OrderBy(col => interactive.ToList().IndexOf(col.gameObject.layer)).ToList();

            foreach (var col in _potentialTargets)
            {
                Debug.Log($"{col.gameObject.name} && {col.TryGetComponent(out IInteractive aa)}");

                if (col.Other().TryGetComponent(out IInteractive interactive) && interactive.Interact(_rb))
                {
                    _current = interactive;
                    _rotation = transform.rotation;
                    return;
                }
            }
        }
        
        private void Interact(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                Interact();
            }
            else if (_current != null)
            {
                _current.InteractRelease(_rb);
                _current = null;
            }
        }

    }
}

/*
public abstract class Item : MonoBehaviour
{
    public void Interact()
    {
        if (TryGetComponent(out Equipment equipment))
        {
            if (equipment.IsEquipped)
            {
                // Unequip
                equipment.Unequip();
            }
            else
            {
                // Equip
                equipment.Equip(this);
            }
        }
    }
}

public class Key : Item
{
    [FormerlySerializedAs("keyses")]
    [SerializeField]
    [Tooltip("Locks that can be opened by this key.")]
    private Keys locks;

    public bool CanOpen(Keys keyses)
    {
        return (this.locks & keyses) != 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out ILockable lockable) && CanOpen(lockable.AcceptedKeys))
        {
            // Open lock
        }
       
    }
}
*/
internal interface ILockable
{
    public Keys AcceptedKeys { get; }
}


/// <summary>
/// Locks that can be opened by keys.
/// </summary>
[Flags]
public enum Keys
{
    Nothing    = 0,
    Everything = ~0,
    Red        = 1 << 0,
    Green      = 1 << 1,
    Blue       = 1 << 2,
    Yellow     = 1 << 3,
}