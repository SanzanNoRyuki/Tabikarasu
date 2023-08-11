using System.Collections;
using System.Collections.Generic;
using Environment.Interactive.Doors;
using UnityEngine;
using Utility.Attributes;


public class Door : ProgressCondition
{
    [field: Fixed]
    [field: SerializeField]
    public bool AreOpen { get; private set; }
    
    [SerializeField]
    [Tooltip("The door's lock.")]
    private Lock @lock;

    [SerializeField]
    private Transform openPosition;

    [SerializeField]
    private Transform closedPosition;

    private Coroutine _doorsCoroutine;

    [SerializeField]
    private float speed = 1f;


    private void Awake()
    {
        if (AreOpen)
        {
            Open(true);
        }
        else
        {
            Close();
        }
    }

    public override bool ConditionMet()
    {
        return AreOpen;
    }

    
    public void Open()
    {
        Open(true);
    }
    
    public void Open(bool overrideLock)
    {
        if (!overrideLock && @lock != null && @lock.IsLocked()) return;

        _doorsCoroutine = StartCoroutine(DoorsCoroutine(openPosition));
    }

    [ContextMenu("Close")]
    public void Close()
    {
        _doorsCoroutine = StartCoroutine(DoorsCoroutine(closedPosition));
    }

    [ContextMenu("Open")]
    private void OpenEditor()
    {
        Open(true);
    }

    private IEnumerator DoorsCoroutine(Transform position)
    {
        if (_doorsCoroutine != null) StopCoroutine(_doorsCoroutine);

        while (Vector3.Distance(transform.position, position.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, position.position, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}