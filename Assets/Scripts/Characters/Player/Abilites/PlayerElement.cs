using System;
using Elements;
using Input_System.Game_Controller;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility.Extensions;
using Utility.Save_System;
using Utility.Save_System.Data;

namespace Characters.Player.Abilites
{
    /// <summary>
    /// Element of the player.
    /// </summary>
    public class PlayerElement : ElementComponent, IPersistentDataHandler
    {
        public void Unlock(Elements.Element element)
        {
            unlocked |= (Elements.Elements) element;
        }

        public void Unlock(Elements.Elements element)
        {
            unlocked |= element;
        }

        
        [SerializeField]
        private Elements.Elements unlocked;

        public bool SwitchElement(Elements.Element newElement)
        {
            return ElementsExtensions.HasFlag(unlocked, newElement) && base.ChangeElement(newElement);
        }

        /// <summary>
        /// Prevents player from switching to <see cref="element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void LockElement(Elements.Element element)
        {
            unlocked &= element switch
                        {
                            Elements.Element.Air   => ~Elements.Elements.Air,
                            Elements.Element.Earth => ~Elements.Elements.Earth,
                            Elements.Element.Water => ~Elements.Elements.Water,
                            Elements.Element.Fire  => ~Elements.Elements.Fire,
                            _                      => throw new ArgumentOutOfRangeException(nameof(element), element, null)
                        };
        }

        public void UnlockElement(Elements.Element element)
        {
            unlocked |= element switch
                        {
                            Elements.Element.Air   => Elements.Elements.Air,
                            Elements.Element.Earth => Elements.Elements.Earth,
                            Elements.Element.Water => Elements.Elements.Water,
                            Elements.Element.Fire  => Elements.Elements.Fire,
                            _                      => throw new ArgumentOutOfRangeException(nameof(element), element, null)
                        };
        }

        private void Start()
        {
            //Current = SaveSystemManager.GameData.Element;
            // Enable current element by default.
            UnlockElement(Current);
        }

        private void OnEnable()
        {
            //GameController.Subscribe(GameController.Actions.Transform,    ParseTransformInput);
            GameController.Subscribe(GameController.Actions.PrevElement,  ParsePrevInput);
            GameController.Subscribe(GameController.Actions.AirElement,   ParseAirInput);
            GameController.Subscribe(GameController.Actions.WaterElement, ParseWaterInput);
            GameController.Subscribe(GameController.Actions.EarthElement, ParseEarthInput);
            GameController.Subscribe(GameController.Actions.FireElement,  ParseFireInput);
        }

        private void OnDisable()
        {
            //GameController.Unsubscribe(GameController.Actions.Transform, ParseTransformInput);
            GameController.Unsubscribe(GameController.Actions.PrevElement,  ParsePrevInput);
            GameController.Unsubscribe(GameController.Actions.AirElement,   ParseAirInput);
            GameController.Unsubscribe(GameController.Actions.WaterElement, ParseWaterInput);
            GameController.Unsubscribe(GameController.Actions.EarthElement, ParseEarthInput);
            GameController.Unsubscribe(GameController.Actions.FireElement,  ParseFireInput);
        }

        private void ParsePrevInput(InputAction.CallbackContext obj)
        {
            SwitchElement(Previous);
        }

        private void ParseAirInput(InputAction.CallbackContext obj)
        {
            SwitchElement(Elements.Element.Air);
        }

        private void ParseWaterInput(InputAction.CallbackContext obj)
        {
            SwitchElement(Elements.Element.Water);
        }

        private void ParseEarthInput(InputAction.CallbackContext obj)
        {
            SwitchElement(Elements.Element.Earth);
        }

        private void ParseFireInput(InputAction.CallbackContext obj)
        {
            SwitchElement(Elements.Element.Fire);
        }
        /*

        [FormerlySerializedAs("guiMenu")]
        [SerializeField]
        private PauseMenu pauseMenu;

        [SerializeField]
        private GameObject airGui;

        [SerializeField]
        private GameObject waterGui;

        [SerializeField]
        private GameObject fireGui;

        [SerializeField]
        private GameObject earthGui;


        private void ParseTransformInput(InputAction.CallbackContext obj)
        {

            if (obj.ReadValueAsButton())
            {
                GameController.StartTransforming();

                if (pauseMenu != null) pauseMenu.Open();
                // Enable Gui
            }
            else
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;

                if (selected != null)
                {
                    if (selected == airGui)
                    {
                        SwitchElement(Elements.Element.Air);
                    }
                    else if (selected == waterGui)
                    {
                        SwitchElement(Elements.Element.Water);
                    }
                    else if (selected == fireGui)
                    {
                        SwitchElement(Elements.Element.Fire);
                    }
                    else if (selected == earthGui)
                    {
                        SwitchElement(Elements.Element.Earth);
                    }
                }

                GameController.StopTransforming();
                if (pauseMenu != null) pauseMenu.Close();

                

                // Switch to selected element
            }
        }
        */

        public void Save(ref GameData gameData)
        {
            gameData.Element = Current;
        }

        public void Load(GameData gameData)
        {
            ChangeElement(gameData.Element);
        }
    }
}