using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Key : MonoBehaviour
{
    public const int MinCode = 0;
    public const int MaxCode = 100;

    [Range(MinCode, MaxCode)]
    [SerializedField]
    private int keyCode;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ILock @lock)) @lock.Unlock(keyCode);       
    }
}

public interface ILock
{
    public bool IsLocked();

    public void Lock();

    public bool Unlock();

    public bool Unlock(int keyCode);
}

public class Lock : ILock
{
    [Readonly]
    [SerializedField]
    private bool locked;

    [Range(Key.MinCode, Key.MaxCode)]
    [SerializedField]
    private int keyCode;

    public bool IsLocked()
    {
        return locked;
    }

    [ContextMenu("Lock")]
    public void Lock()
    {

    }

    [ContextMenu("Unlock")]
    public bool Unlock()
    {
        Unlock(code);
    }

    public bool Unlock(int code)
    {
        if (code != keyCode) return;

        Unlocked?.Invoke();
    }
}
*/