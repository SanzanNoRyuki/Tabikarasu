using System;
using System.Collections.Generic;
using Characters.Healths;
using Characters.LifeCycle;
using Elements;
using Environment.Decorations;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.Player.Death
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(ElementComponent))]
    public class PlayerCorpseHandler : MonoBehaviour
    {
        [Header("Elemental remains")]
        
        [SerializeField]
        private Corpse air;
        private readonly Queue<Corpse> _airCorpses = new();

        [SerializeField]
        private Corpse water;
        private readonly Queue<Corpse> _waterCorpses = new();

        [SerializeField]
        private Corpse earth;
        private readonly Queue<Corpse> _earthCorpses = new();

        [SerializeField]
        private Corpse fire;
        private readonly Queue<Corpse> _fireCorpses = new();

        private Health           _health;
        private ElementComponent _element;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _element = GetComponent<ElementComponent>();
        }

        private void OnEnable()
        {
            UpdateQueues();

            _health.Died                    += SpawnCorpse;
            LifeCycleManager.LivesIncreased += UpdateQueues;
        }

        private void OnDisable()
        {
            _health.Died                    -= SpawnCorpse;
            LifeCycleManager.LivesIncreased -= UpdateQueues;
        }
        
        private void UpdateQueues()
        {
            UpdateQueue(_airCorpses,   air);
            UpdateQueue(_waterCorpses, water);
            UpdateQueue(_earthCorpses, earth);
            UpdateQueue(_fireCorpses,  fire);
            
        }

        private void UpdateQueue(Queue<Corpse> corpses, [CanBeNull] Corpse prefab)
        {
            if (corpses.Count < LifeCycleManager.Lives() + 1)
            {
                if (prefab == null) return;

                for (int i = 0; i < LifeCycleManager.Lives() + 1 - corpses.Count; i++)
                {
                    Corpse corpse = Instantiate(prefab);
                    corpse.gameObject.SetActive(false);
                    corpses.Enqueue(corpse);
                }
            }
            else if (corpses.Count > LifeCycleManager.Lives() + 1)
            {
                for (int i = 0; i < corpses.Count - (LifeCycleManager.Lives() + 1); i++)
                {
                    Corpse corpse = corpses.Dequeue();
                    Destroy(corpse.gameObject);
                }
            }
        }


        private void SpawnCorpse()
        {
            switch (_element.Current)
            {
                case Elements.Element.Air:
                    SpawnCorpse(_airCorpses);
                    break;
                case Elements.Element.Water:
                    SpawnCorpse(_waterCorpses);
                    break;
                case Elements.Element.Earth:
                    SpawnCorpse(_earthCorpses);
                    break;
                case Elements.Element.Fire:
                    SpawnCorpse(_fireCorpses);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SpawnCorpse(Queue<Corpse> corpses)
        {
            Corpse    corpse = corpses.Dequeue();
            corpses.Enqueue(corpse);

            Transform tf = transform;
            corpse.Spawn(tf.position, tf.rotation);
        }
    }
}