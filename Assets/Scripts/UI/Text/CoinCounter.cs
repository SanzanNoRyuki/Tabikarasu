using Environment.Collectibles.Tokens;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Utility.Attributes;

namespace UI.Text
{
    /// <summary>
    /// Dynamic text counter.
    /// </summary>
    public class CoinCounter : DynamicCounter
    {
        private void OnEnable()
        {
            Coin.CountChanged += OnValueChanged;
        }
        
        private void OnDisable()
        {
            Coin.CountChanged -= OnValueChanged;
        }

        private void OnValueChanged()
        {
            Value = Coin.Count;
            Redraw();
        }
    }
}
