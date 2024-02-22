using System;
using Elements;
using UnityEngine;
using Utility.Save_System;
using Utility.Save_System.Data;

namespace Characters.Player.Skills.Elemental
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ElementComponent))]
    public class PlayerPassive : MonoBehaviour, IPersistentDataHandler
    {
        [Header("Unlockable")]

        [SerializeField]
        [Tooltip("Unlocked passive elements.")]
        private Elements.Elements unlocked;

        [SerializeField]
        [Tooltip("Unlockable air passive")]
        private GameObject air;

        [SerializeField]
        [Tooltip("Unlockable water passive")]
        private GameObject water;

        [SerializeField]
        [Tooltip("Unlockable fire passive")]
        private GameObject fire;

        [SerializeField]
        [Tooltip("Unlockable earth passive")]
        private GameObject earth;

        [Header("Permanent")]

        [SerializeField]
        [Tooltip("Permanent air passives.")]
        private GameObject[] permanentAir;

        [SerializeField]
        [Tooltip("Permanent water passives.")]
        private GameObject[] permanentWater;

        [SerializeField]
        [Tooltip("Permanent fire passives.")]
        private GameObject[] permanentFire;

        [SerializeField]
        [Tooltip("Permanent earth passives.")]
        private GameObject[] permanentEarth;


        private ElementComponent _element;

        private void Awake()
        {
            _element = GetComponent<ElementComponent>();
        }

        private void OnEnable()
        {
            _element.Changed += OnElementChanged;
        }

        private void OnDisable()
        {
            _element.Changed -= OnElementChanged;
        }

        private void OnElementChanged(Element element)
        {
            DisableAll();
            /*if (unlocked.HasFlag((Elements.Elements) element))*/ Enable(element);
        }
        
        private void Enable(Element element)
        {
            switch (element)
            {
                case Element.Air:
                    if (air != null) air.SetActive(true);
                    foreach (GameObject passive in permanentAir) passive.SetActive(true);
                    break;
                case Element.Water:
                    if (water != null) water.SetActive(true);
                    foreach (GameObject passive in permanentWater) passive.SetActive(true);
                    break;
                case Element.Fire:
                    if (fire != null) fire.SetActive(true);
                    foreach (GameObject passive in permanentFire) passive.SetActive(true);
                    break;
                case Element.Earth:
                    if (earth != null) earth.SetActive(true);
                    foreach (GameObject passive in permanentEarth) passive.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element), element, null);
            }
        }

        private void DisableAll()
        {
            // Disable all unlockable passives.
            if (air != null) air.SetActive(false);
            if (water != null) water.SetActive(false);
            if (fire != null) fire.SetActive(false);
            if (earth != null) earth.SetActive(false);

            // Disable all permanent passives.
            foreach (GameObject passive in permanentAir)   passive.SetActive(false);
            foreach (GameObject passive in permanentWater) passive.SetActive(false);
            foreach (GameObject passive in permanentFire)  passive.SetActive(false);
            foreach (GameObject passive in permanentEarth) passive.SetActive(false);
        }


        public void Unlock(Element element)
        {
            Debug.Log("Unlocking " + element);
            Unlock((Elements.Elements) element);
        }

        private void Unlock(Elements.Elements element)
        {
            unlocked |= element;
            Enable((Element) element);
        }

        [ContextMenu("Unlock Air")]
        public void UnlockAir()
        {
            Unlock(Elements.Elements.Air);
        }

        [ContextMenu("Unlock Water")]
        public void UnlockWater()
        {
            Unlock(Elements.Elements.Water);
        }

        [ContextMenu("Unlock Fire")]
        public void UnlockFire()
        {
            Unlock(Elements.Elements.Fire);
        }

        [ContextMenu("Unlock Earth")]
        public void UnlockEarth()
        {
            Unlock(Elements.Elements.Earth);
        }

        // On element switch check if current element is unlocked and if yes then switch to it.
        public void Save(ref GameData gameData)
        {
            gameData.AirPassive = unlocked.HasFlag(Elements.Elements.Air);
            gameData.WaterPassive = unlocked.HasFlag(Elements.Elements.Water);
            gameData.FirePassive = unlocked.HasFlag(Elements.Elements.Fire);
            gameData.EarthPassive = unlocked.HasFlag(Elements.Elements.Earth);

        }

        private void LockAll()
        {
            unlocked = 0;
        }

        public void Load(GameData gameData)
        {
            return;
            LockAll();

            if (gameData.AirPassive)   UnlockAir();
            if (gameData.WaterPassive) UnlockWater();
            if (gameData.FirePassive) UnlockFire();
            if (gameData.EarthPassive) UnlockEarth();
        }
    }
}