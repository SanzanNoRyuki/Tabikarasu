using Attacks.Affectables;
using Characters.Healths;
using TMPro;
using UnityEngine;
using Utility.Constants;
using Utility.Save_System;
using Utility.Save_System.Data;

namespace UI.Text
{
    /// <summary>
    /// Dynamic text counter. Don't use, player tracks deaths by himself.
    /// </summary>
    public class DeathCounter : DynamicCounter, IPersistentDataHandler
    {
        private void Add()
        {
            Value++;
            Redraw();
        }

        private Health _health;

        private void OnEnable()
        {
            GameObject player = GameObject.FindGameObjectWithTag(Tags.Player);

            if (player != null && player.TryGetComponent(out _health))
            {
                _health.Died += Add;
            }
            else
            {
                Debug.LogWarning("Death counter requires player with health component", this);
                enabled = false;
            }
        }
        
        private void OnDisable()
        {
            if (_health != null) _health.Died -= Add;
        }

        public void Save(ref GameData gameData)
        {
            gameData.Deaths = Value;
        }

        public void Load(GameData gameData)
        {
            Value = gameData.Deaths;
            Redraw();
        }
    }
}