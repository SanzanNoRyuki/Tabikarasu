using System;
using Environment.Interactive.Doors;
using UnityEngine;
using UnityEngine.Events;

public class Lock : ProgressCondition
{
    public event Action OnLock;
    public event Action OnUnlock;

    [SerializeField]
    private UnityEvent onLock;

    [SerializeField]
    private UnityEvent onUnlock;

    public virtual bool IsLocked()
    {
        return enabled;
    }

    private void OnEnable()
    {
        OnLock?.Invoke();
        onLock.Invoke();
    }

    protected void OnDisable()
    {
        OnUnlock?.Invoke();
        onUnlock.Invoke();
    }

    public override bool ConditionMet()
    {
        return !IsLocked();
    }
}