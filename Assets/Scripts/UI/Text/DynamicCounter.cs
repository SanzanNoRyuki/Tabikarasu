using System;
using Characters;
using TMPro;
using UnityEngine;
using Utility.Attributes;

namespace UI.Text
{
    /// <summary>
    /// Dynamic text counter.
    /// </summary>
    public class DynamicCounter : MonoBehaviour
    {
        [field: SerializeField]
        public int Value { get; protected set; }

        [field: SerializeField]
        public int Digits { get; private set; } = 4;

        [field: SerializeField]
        public float MonoSpace { get; private set; } = 0.5f;

        [field: SerializeField]
        public string TextBeforeValue { get; private set; } = "";

        [field: SerializeField]
        public string TextAfterValue { get; private set; } = "";

        private TMP_Text _textField;

        private void Awake()
        {
            _textField = GetComponent<TMP_Text>();
        }

        [ContextMenu("Redraw")]
        public void Redraw()
        {
            if (!Application.isPlaying) return;

            var monospace = MonoSpace.ToString(System.Globalization.CultureInfo.InvariantCulture);
            _textField.text = $"{TextBeforeValue}<mspace={monospace}em>{Value.ToString().PadLeft(Digits, ' ')}</mspace>{TextAfterValue}";
        }
    }
}
