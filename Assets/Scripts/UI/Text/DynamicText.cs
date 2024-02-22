using TMPro;
using UnityEngine;

namespace UI.Text
{
    /// <summary>
    /// Dynamically modified text.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public abstract class DynamicText : MonoBehaviour
    {
        protected TMP_Text TextField;

        protected virtual void Awake()
        {
            TextField = GetComponent<TMP_Text>();
        }
    }
}