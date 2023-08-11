using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Ability attack which can be casted or released.
/// </summary>
[DisallowMultipleComponent]
public class Ability : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Cast on release instead of press.")]
    private bool castOnRelease = false;

    [FormerlySerializedAs("duration")]
    [SerializeField]
    [Range(0f, 10f)]
    [Tooltip("Required hold duration for succesful the cast.")]
    private float holdDuration;
    private float _pressTime;

    [SerializeField]
    [Range(0f, 10f)]
    [Tooltip("Delay before casting after release.")]
    private float delay;

    [SerializeField]
    [Range(0f, 10f)]
    [Tooltip("Press/release wont count if ability was casted recently.")]
    private float cooldown;

    [SerializeField]
    [Tooltip("Ability was pressed.")]
    private UnityEvent pressed;

    [SerializeField]
    [Tooltip("Ability was released.")]
    private UnityEvent released;
    
    [SerializeField]
    [Tooltip("Ability was cast and succeeded.")]
    private UnityEvent castSuccess;

    [SerializeField]
    [Tooltip("Ability was cast but failed.")]
    private UnityEvent castFailure;

    public event Action Pressed;
    public event Action Released;
    public event Action CastSuccess;
    public event Action CastFailure;

    private bool  _castInProgress;
    private float _lastCastTime;
    private Coroutine _holdCoroutine;

    public  bool  OnCooldown => Time.time - _lastCastTime <= cooldown;

    public bool IsCasting { get; private set; }

    private void OnValidate()
    {
        if (holdDuration > 0f) castOnRelease = true;
    }

    [ContextMenu("Press and hold.")]
    public void Press()
    {
        _pressTime = Time.time;

        if (!castOnRelease)
        {
            if (_castInProgress || OnCooldown) return;

            pressed.Invoke();
            Pressed?.Invoke();


            _castInProgress = true;
            DOVirtual.DelayedCall(delay, () =>
            {
                _castInProgress = false;
                Cast();
            });
        }
        else
        {
            IsCasting = true;

            pressed.Invoke();
            Pressed?.Invoke();

            if (_holdCoroutine != null) StopCoroutine(_holdCoroutine);
            _holdCoroutine = StartCoroutine(Hold());
        }

    }

    [ContextMenu("Release")]
    public void Release()
    {
        released.Invoke();
        Released?.Invoke();
        
        if (!IsCasting) return;
        
        IsCasting = false;
        Cast();
    }

    [ContextMenu("Cast")]
    public void Cast()
    {
        IsCasting = false;

        _lastCastTime = Time.time;
        
        if (holdDuration <= 0f || Time.time - _pressTime >= holdDuration)
        {
            castSuccess.Invoke();
            CastSuccess?.Invoke();
        }
        else
        {
            castFailure.Invoke();
            CastFailure?.Invoke();
        }
    }

    private IEnumerator Hold()
    {
        yield return new WaitForSeconds(holdDuration);

        if (IsCasting)
        {
            Cast();
        }
    }
}