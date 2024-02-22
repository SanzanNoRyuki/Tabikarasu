using System;
using Attacks.Abilities;
using Elements;
using Input_System;
using Input_System.Game_Controller;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility.Attributes;

namespace Characters.Behaviours
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ElementComponent))]
    public class PlayerSpell : MonoBehaviour
    {
        public bool IsCasting => active != null && active.IsCasting;
        
        [ReadOnly]
        [SerializeField]
        private Ability active;

        [SerializeField]
        private Ability air;

        [SerializeField]
        private Ability water;

        [SerializeField]
        private Ability earth;

        [SerializeField]
        private Ability fire;

        private ElementComponent _element;

        private void Awake()
        {
            _element = GetComponent<ElementComponent>();
        }

        private void Start()
        {
            SwitchSpell(_element.Current);
        }

        private void OnEnable()
        {
            GameController.Subscribe(GameController.Actions.Spell, ParseSkillInput);
            _element.Changed += SwitchSpell;
        }

        private void OnDisable()
        {
            GameController.Unsubscribe(GameController.Actions.Spell, ParseSkillInput);
            _element.Changed -= SwitchSpell;
        }

        private void ParseSkillInput(InputAction.CallbackContext obj)
        {
            if (active == null) return;

            if (obj.ReadValueAsButton())
            {
                active.Press();
            }
            else
            {
                active.Release();
            }
        }

        private void SwitchSpell(Elements.Element element)
        {
            active = element switch
                      {
                          Elements.Element.Air   => air,
                          Elements.Element.Water => water,
                          Elements.Element.Earth => earth,
                          Elements.Element.Fire  => fire,
                          _                      => active,
                      };
        }
    }
}
