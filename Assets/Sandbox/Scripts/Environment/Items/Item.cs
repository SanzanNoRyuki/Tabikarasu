using System.Collections;
using System.Collections.Generic;
using Attacks.Affectables;
using UnityEngine;

/// <summary>
/// Pickable item.
/// </summary>
public class Item : MonoBehaviour, IInteractive
{
    public bool IsPicked { get; private set; }

    public bool Interact(Rigidbody2D interactor)
    {
        PickUp(interactor);
        return true;
    }

    private void PickUp(Rigidbody2D interactor)
    {
        
    }

    public bool InteractRelease(Rigidbody2D interactor)
    {
        return false;
    }
}
