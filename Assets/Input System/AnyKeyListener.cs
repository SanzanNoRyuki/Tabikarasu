using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Input_System
{
    /// <summary>
    /// Listens to any button input from all devices.
    /// </summary>
    public class AnyKeyListener : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Delay after which listener starts listening.")]
        private float delay;

        private IDisposable _eventListener;
        private Coroutine   _delayCoroutine;
        
        [SerializeField]
        [Tooltip("Event invoked when any key is pressed.")]
        private UnityEvent<InputControl> anyKeyPressed;

        private void OnEnable()
        {
            if (_delayCoroutine != null) StopCoroutine(_delayCoroutine);

            if (delay > 0f)
            {
                _delayCoroutine = StartCoroutine(DelayCoroutine());
            }
            else
            {
                _eventListener = InputSystem.onAnyButtonPress.Call(OnAnyKey);
            }
        }

        private void OnDisable()
        {
            _eventListener?.Dispose();
        }

        private System.Collections.IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(delay);

            _eventListener = InputSystem.onAnyButtonPress.Call(OnAnyKey);
        }

        private void OnAnyKey(InputControl button)
        {
            if (button.device is Keyboard && button.name == "escape") return;
            if (button.device is Gamepad  && button.name == "start") return;
            if (button.device is Gamepad  && button.name == "select") return;
            
            anyKeyPressed?.Invoke(button);
        }
    }
}