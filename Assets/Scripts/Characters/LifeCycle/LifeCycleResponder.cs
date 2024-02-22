using Attacks.Affectables;
using Characters.Healths;
using Characters.LifeCycle;
using UnityEngine;
using Utility.Attributes;

namespace LifeCycle
{
    /// <summary>
    /// Revives after a <see cref="LifeCycleManager.Lives">lives</see> amount of player deaths.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Health))]
    public class LifeCycleResponder : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        [Tooltip("Turns until respawn.")]
        private int counter;

        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            counter = LifeCycleManager.Lives();

            LifeCycleManager.Ticked         += OnTick;
            LifeCycleManager.LivesIncreased += DelayCount;
        }

        private void OnDestroy()
        {
            LifeCycleManager.Ticked -= OnTick;
            LifeCycleManager.LivesIncreased -= DelayCount;
        }

        private void OnTick()
        {
            if (_health.IsAlive) return;
            if (--counter > 0)   return;
            
            _health.Respawn();
            counter = LifeCycleManager.Lives();
        }

        [ContextMenu("Delay Count")]
        private void DelayCount()
        {
            counter++;
        }
    }
}