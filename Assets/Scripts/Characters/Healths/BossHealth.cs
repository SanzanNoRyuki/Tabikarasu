using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Healths
{
    /// <summary>
    /// Boss health is sustained by shamans.
    /// </summary>
    public class BossHealth : MonoBehaviour
    {
        [SerializeField]
        private List<Health> shamans = new();

        [SerializeField]
        private UnityEvent hurt;

        [SerializeField]
        private UnityEvent slain;

        private void OnEnable()
        {
            foreach (Health shaman in shamans) shaman.Died += OnShamanDeath;
        }

        private void OnDisable()
        {
            foreach (Health shaman in shamans) shaman.Died -= OnShamanDeath;
        }

        private void OnShamanDeath()
        {
            if (shamans.Any(shaman => shaman.IsAlive))
            {
                hurt.Invoke();
            }
            else
            {
                slain.Invoke();
                Debug.Log("Slain");
            }
        }
    }
}
