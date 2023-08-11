using System.Collections;
using System.Collections.Generic;
using Characters.Behaviour;
using Characters.Behaviours;
using UnityEngine;
using Pathfinding;
using Utility.Attributes;
using Utility.Extensions;


[RequireComponent(typeof(Target))]
public class SkyChaser : EntityBehaviour
{
    private Target _target;

    [SerializeField]
    private float speed = 100f;

    [SerializeField]
    private float nextWaypointDistance = 0.1f;

    private Path _path;
    private int _currentWaypoint;
    private bool _reachedEndOfPath;

    private Seeker _seeker;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _target = GetComponent<Target>();
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        InvokeRepeating(nameof(UpdatePath), 0f, repathRate);
    }

    [Fixed]
    [SerializeField]
    private float repathRate = 0.5f;



    private void UpdatePath()
    {
        if (_target.Transform == null) return;

        if (!_seeker.IsDone())
            return;

        // Calculate the distance between the SkyChaser and the target
        float distanceToTarget = Vector2.Distance(_rb.position, _target.Transform.position);

        // Check if the distance is greater than the nextWaypointDistance
        if (distanceToTarget > nextWaypointDistance)
        {
            _seeker.StartPath(_rb.position, _target.Transform.position, OnPathComplete);
        }
    }


    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path            = p;
            _currentWaypoint = 0;
        }
    }


    

    private void Update()
    {
        if (_target.Transform == null) return;

        if (_path == null)
        {
            return;
        }

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            return;
        }
        else
        {
            _reachedEndOfPath = false;
        }

        if (_reachedEndOfPath) return;

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force     = direction * speed * Time.deltaTime;

        _rb.AddForce(force);

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            _currentWaypoint++;
        }
    }


}
