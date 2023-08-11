using System;
using Characters.Player.Skills.Elemental;
using Elements;
using UnityEngine;
using Utility.Save_System;
using Utility.Save_System.Data;

namespace Environment.Collectibles.Tokens
{
    [RequireComponent(typeof(ElementComponent))]
    public class Gem : Token, IPersistentDataHandler
    {
        public static int  Count { get; private set; }

        private ElementComponent _element;

        public static event Action CountChanged;

        private void Awake()
        {
            _element = GetComponent<ElementComponent>();
        }

        private void OnEnable()
        {
            Collected += OnCollect;
        }

        private void OnDisable()
        {
            Collected -= OnCollect;
        }

        private void OnCollect(GameObject collector)
        {
            if (collector == null) return;
            if (collector.TryGetComponent(out PlayerPassive passive)) passive.Unlock(_element.Current);
        }

        public void Save(ref GameData gameData)
        {
            gameData.Gems = Count;
        }

        public void Load(GameData gameData)
        {
            Count = gameData.Gems;
        }

        public override void Add(int val)
        {
            Count += val;
            CountChanged?.Invoke();
        }
    }
}