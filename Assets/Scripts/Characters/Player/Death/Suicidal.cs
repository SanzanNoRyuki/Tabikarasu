using Characters.Healths;
using Input_System.Game_Controller;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.Skills
{
    /// <summary>
    /// Player suicide module. Just like me after this shit fest.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Health))]
    public class Suicidal : MonoBehaviour
    {
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }
        private void OnEnable()
        {
            GameController.Subscribe(GameController.Actions.Suicide, ParseSuicideInput);
        }

        private void OnDisable()
        {
            GameController.Unsubscribe(GameController.Actions.Suicide, ParseSuicideInput);
        }

        private void ParseSuicideInput(InputAction.CallbackContext obj)
        {
            _health.Kill();
        }
    }
}