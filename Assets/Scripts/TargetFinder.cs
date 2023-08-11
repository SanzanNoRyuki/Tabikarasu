using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Behaviours;
using UnityEngine;
using UnityEngine.Serialization;
using Utility.Constants;
using Utility.Extensions;


/// <summary>
/// Defends its spawn point.
/// </summary>
public class SpawnDefender : MonoBehaviour
{
    Vector3 _spawnPoint;

    [SerializeField]
    private float range = 5f;

    [SerializeField]
    private float updateRate = 0.5f;
    private float _updateCounter = 0f;

    private Vector3 _target;

    private void Awake()
    {
        _spawnPoint = transform.position;
        _target = _spawnPoint;
    }

    private void Update()
    {
        if (_updateCounter <= 0f)
        {
            UpdateTarget();
            _updateCounter = updateRate;
        }
        else
        {
            _updateCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _target) <= 0.1f) return;

        // Replace with AI
        transform.position = Vector3.MoveTowards(transform.position, _target, 1f * Time.fixedDeltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_spawnPoint, range);
    }

    private void UpdateTarget()
    {
        _target = _spawnPoint;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_spawnPoint, range);

        if (colliders.Length == 0) return;

        foreach (Collider2D collider in colliders)
        {
            GameObject other = collider.Other();

            if (other.CompareTag(Tags.Player))
            {
                _target = other.transform.position;
                break;
            }
        }
    }
}

public class TargetFinder : MonoBehaviour
{
    [field: SerializeField]
    public Transform DefaultTarget { get; private set; }

    private Target _target;

    //[SerializeField]
    //private float sameTargetTimer = 5f;

    private float _sameTargetCounter = 0f;

    [SerializeField]
    private Range acquisition;

    [SerializeField]
    private Range loss;

    [SerializeField]
    [Tooltip("Layers that can be interacted with. Order is important. Lower priority layers will be interacted with first.")]
    private List<LayerMask> possibleTargets;
    private ContactFilter2D _possibleTargets;

    private List<Collider2D> _potentialTargets;

    private Transform _lastTarget;

    [Serializable]
    private class Range
    {
        public bool  frontOnly;
        public float range;
    }

    private void OnValidate()
    {
        loss.range = Mathf.Max(loss.range, acquisition.range);

        // Ensure only one layer is selected per mask
        for (int i = 0; i < possibleTargets.Count; i++) possibleTargets[i] = possibleTargets[i].Unique();

        if (!Application.isPlaying) return;

        // Remove nothing layers
        possibleTargets = possibleTargets.Where(layer => layer != 0).ToList();

        // Ensure priority order is unique
        possibleTargets = possibleTargets.Distinct().ToList();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, acquisition.range);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(position, loss.range);
    }

    private Coroutine _search;

    [SerializeField]
    private float updateRate;
    private float waitTime;

    [SerializeField]
    private bool onlyFromFront;


    private void Awake()
    {
        _target = GetComponent<Target>();
        _potentialTargets = new List<Collider2D>();

        _possibleTargets = new ContactFilter2D
        {
            useTriggers  = false,
            useLayerMask = true,
            layerMask    = possibleTargets.Aggregate(0, (current, layer) => current | layer.value),
        };
    }

    private void Update()
    {
        if (_target.HasTarget && _target.Transform != DefaultTarget)
        {
            LoseTarget();
        }
        else
        {
            FindTarget();
        }
    }

    private void LoseTarget()
    {
        if (Vector2.Distance(transform.position, _target.Transform.position) < loss.range) _target.Clear();


        _target.Clear();

        

        //if (loss.frontOnly && !transform.Facing(_target.Get.transform)) _target.Clear();


        //if (Vector2.Distance(transform.position, _target.Get.position) > loss.range) _target.Clear();
    }

    private void FindTarget()
    {
        _potentialTargets.Clear();
        Physics2D.OverlapCircle(transform.position, acquisition.range, _possibleTargets, _potentialTargets);

        Transform closest = DefaultTarget;
        
        if (_potentialTargets.Count == 0)
        {
            // Already set to default target
        }
        else if (_potentialTargets.Count == 1 && _potentialTargets[0].transform != _lastTarget || _sameTargetCounter > Time.time)
        {
            closest = _potentialTargets[0].transform;
        }
        else
        {
            
            /*
            // Sort colliders by priority
            _potentialTargets = _potentialTargets.OrderBy(col => possibleTargets.ToList().IndexOf(col.gameObject.layer)).ToList();

            for (int i = 1; i < _potentialTargets.Count; i++)
            {
                //If potenial target layer is lower on list than current closest, break
                if (possibleTargets.IndexOf(_potentialTargets[i].gameObject.layer) > possibleTargets.IndexOf(closest.gameObject.layer)) break;

                if (Vector2.Distance(transform.position, _potentialTargets[i].transform.position) < Vector2.Distance(transform.position, closest.position))
                {
                    if (sameTargetCounter > Time.time && _potentialTargets[i].transform == _lastTarget) continue;

                    closest = _potentialTargets[i].transform;
                }
            }
            */
        }

        _target.Set(closest);
    }


}