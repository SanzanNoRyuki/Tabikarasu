using Characters.LifeCycle;
using Environment.Decorations;
using UnityEngine;
using Utility.Attributes;

namespace Characters.Player.Death
{
    public class PlayerCorpse : Corpse
    {
        [ReadOnly]
        [SerializeField]
        [Tooltip("Turns until despawn.")]
        private int counter;

        private bool _firstCorpseHandled;

        private void Awake()
        {
            Spawned   += OnSpawn;
            Despawned += OnDespawn;

            counter = LifeCycleManager.Lives();
        }
        
        private void OnDestroy()
        {
            Spawned   -= OnSpawn;
            Despawned -= OnDespawn;
        }

        private void OnSpawn()
        {
            counter                         =  LifeCycleManager.Lives();
            LifeCycleManager.LivesIncreased += DelayCount;
            LifeCycleManager.Ticked         += OnTick;
        }

        private void OnDespawn()
        {
            counter                         =  0;
            LifeCycleManager.LivesIncreased -= DelayCount;
            LifeCycleManager.Ticked         -= OnTick;
        }

        private void OnTick()
        {
            if (--counter <= 0) Despawn();
        }


        [ContextMenu("Delay count")]
        private void DelayCount()
        {
            counter++;
        }
    }
}