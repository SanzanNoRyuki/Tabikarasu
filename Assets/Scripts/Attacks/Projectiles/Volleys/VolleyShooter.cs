using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Attacks.Volleys
{
    /// <summary>
    /// <see cref="VolleySequence">Volley pattern sequence</see> shooter.
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    public class VolleyShooter : MonoBehaviour
    {
        /// <summary>
        /// Default distance between shooters if there is only one shooter.
        /// </summary>
        public const float DefaultDistanceBetweenShooters = 1f;
        
        [SerializeField]
        [Tooltip("Possible volley pattern sequences.")]
        private VolleySequence[] sequences;

        private List<Shooter> _shooters;

        private Coroutine _shootCoroutine;

        private void Awake()
        {
            _shooters         = new List<Shooter>(GetComponentsInChildren<Shooter>());

            PadShooters();
        }

        [ContextMenu("Shoot")]
        public void Shoot()
        {
            if (_shootCoroutine != null) StopCoroutine(_shootCoroutine);
            _shootCoroutine = StartCoroutine(ShootSequence());
        }

        public float GetAverageDistanceBetweenShooters()
        {
            if (_shooters.Count < 2) return DefaultDistanceBetweenShooters;
    
            float xDistance = 0f;
            for (int i = 0; i < _shooters.Count - 1; i++)
            {
                Vector3 position1 = _shooters[i].transform.position;
                Vector3 position2 = _shooters[i + 1].transform.position;
                float   xDist     = Mathf.Abs(position2.x - position1.x);
                xDistance += xDist;
            }

            return xDistance / (_shooters.Count - 1);
        }

        

        

        private void PadShooters()
        {
            if (_shooters.Count < Volley.WaveLength)
            {
                int   additionalShootersNeeded = Volley.WaveLength - _shooters.Count;
                float distanceBetweenShooters  = GetAverageDistanceBetweenShooters();

                for (int i = 0; i < additionalShootersNeeded; i++)
                {
                    if (_shooters.Count == 0)
                    {
                        Debug.LogWarning("Volley shooter cannot function without at least one shooter.", this);
                        return;
                    }

                    Vector3 position   = _shooters.Last().transform.position + Vector3.right * distanceBetweenShooters;
                    Shooter newShooter = Instantiate(_shooters.Last(), position, _shooters.Last().transform.rotation, transform);
                    newShooter.gameObject.name = $"{_shooters[0].gameObject.name} ({_shooters.Count})";

                    _shooters.Add(newShooter);
                }
            }
        }
    
        private IEnumerator ShootSequence()
        {
            if (_shooters.Count == 0 || sequences.Length == 0) yield break;

            VolleySequence volleySequence = GetRandomVolleySequence();
            
            foreach (Volley volley in volleySequence.Volleys)
            {
                foreach (string pattern in volley.Pattern)
                {
                    for (int i = 0; i < _shooters.Count; i++)
                    {
                        if (pattern[i] == volley.ShootGlyph) _shooters[i].Shoot();
                    }

                    yield return new WaitForSeconds(volley.TimeBetweenWaves);
                }
                yield return new WaitForSeconds(volleySequence.TimeBetweenVolleys);
            }
        }

        private VolleySequence GetRandomVolleySequence()
        {
            return sequences[Random.Range(0, sequences.Length)];
        }
    }
}