using System.Collections;
using Characters.Behaviours;
using Pathfinding;
using UnityEngine;
using Utility.Attributes;
using Utility.Constants;

namespace Characters.AI.Targeting
{
    /// <summary>
    /// If enabled, sets <see cref="Target">target</see> to player. Also sets <see cref="AIDestinationSetter">destination</see> to player.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerTarget : MonoBehaviour
    {
        [FixedRange(0f, 5f)]
        [SerializeField]
        [Tooltip("Time between searches for player.")]
        private float waitTime = 0.5f;

        private WaitForSeconds _waitTime;

        private Coroutine _coroutine;

        private void Awake()
        {
            _waitTime = new WaitForSeconds(waitTime);
        }

        private void OnEnable()
        {
            _coroutine = StartCoroutine(SearchPlayer());
        }

        private void OnDisable()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }

        private IEnumerator SearchPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag(Tags.Player);
            while (player == null)
            {
                yield return _waitTime;
                player = GameObject.FindGameObjectWithTag(Tags.Player);
            }

            if (TryGetComponent(out AIDestinationSetter setter)) setter.target = player.transform;
            if (TryGetComponent(out Target target)) target.Set(player.transform);
        }
    }
}
