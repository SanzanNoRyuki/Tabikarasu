using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Miscellaneous.Management
{
    [RequireComponent(typeof(Dropdown))]
    [DisallowMultipleComponent]
    public class AudioDropdown : AudioSelector
    {
        private Dropdown _dropdown;


        // On value changed play sound

        protected void Awake()
        {
            GetComponent<Dropdown>().onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(int arg0)
        {
            //PlayClick();
        }

    }
}