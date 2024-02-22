using System.Collections;
using System.Collections.Generic;
using Attacks.Affectables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;


[RequireComponent(typeof(Splashable))]
[RequireComponent(typeof(Flammable))]
public class TorchKey : Lock
{
    [SerializeField]
    private bool filled = true;

    [SerializeField]
    private UnityEvent onFill;

    [SerializeField]
    private UnityEvent onEmpty;


    [SerializeField] private bool isLit = false;

    [SerializeField]
    private bool conditionIsLit = true;


    public bool IsLit
    {
        get => isLit;
        set => isLit = value;
    }

    public override bool ConditionMet()
    {
        return filled && (isLit == conditionIsLit);
    }

    public override bool IsLocked()
    {
        if (!base.IsLocked()) return false;
        
        return !filled || isLit != conditionIsLit;
    }
}


public class Torch : Item
{

}

public class Key : Item
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out KeyHole keyHole)) keyHole.enabled = false;
    }
}