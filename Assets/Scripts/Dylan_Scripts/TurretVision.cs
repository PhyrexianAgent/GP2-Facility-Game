using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretVision : Perception
{
    [SerializeField] private float _distance = 20f;
    [SerializeField] private float _angle = 12f; // Field of View
    [SerializeField] private float _height = 0f;
    [SerializeField] private int _scanFrequency = 32;
    [SerializeField] private LayerMask _trackableLayers;
    [SerializeField] private LayerMask _occlusionLayers;
    [SerializeField] private List<GameObject> _objectsInSight = new();

    private Collider[] _colliders = new Collider[50];
    private int _count;
    private float _scanInterval;
    private float _scanTimer;

    public bool trackableIsInSight;
    private bool _delayFinished;
    private float _delayLimit = 4f;
    private float _delayTimer;



    protected override void Initialize()
    {
        trackableIsInSight = false;
        _scanInterval = 1.0f / _scanFrequency;
        _delayTimer = _delayLimit;
    }
    protected override void UpdatePerception()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer < 0)
        {
            _scanTimer += _scanInterval;
            Scan();
        }

        UpdateDelay();
    }

    private void Scan()
    {
        _count = Physics.OverlapSphereNonAlloc(transform.position, _distance, _colliders, _trackableLayers, QueryTriggerInteraction.Collide);

        _objectsInSight.Clear();
        if (_count > 0)
        {
            for (int i = 0; i < _count; i++)
            {
                var obj = _colliders[i].gameObject;
                if (IsInSight(obj))
                {
                    _objectsInSight.Add(obj);

                    _delayTimer = 0f;
                    trackableIsInSight = true;
                    _delayFinished = true;
                }
                else
                {
                    _delayFinished = false;
                }
            }
        }
        else // If no one is nearby
        {
            _delayTimer = _delayLimit;
            _delayFinished = false;
        }
    }

    private void UpdateDelay()
    {
        if (!_delayFinished && trackableIsInSight)
        {
            _delayTimer += Time.deltaTime;
            if (_delayTimer >= _delayLimit)
            {
                _delayTimer = _delayLimit;
                trackableIsInSight = false;
                _delayTimer = 0f;
                _delayFinished = true;
            }
        }
    }

    private bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        // Check Height Position
        if (direction.y < -_height || direction.y > _height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);

        // Check if deltaAngle is bigger
        if (deltaAngle > _angle)
        {
            return false;
        }

        destination.y = origin.y;

        // Check if sight is interupted by an object
        if (Physics.Linecast(origin, destination, _occlusionLayers))
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        if (!VisualDebug) { return; }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _distance);
        Gizmos.color = Color.red;
        for (int i = 0; i < _count; i++)
        {
            Gizmos.DrawSphere(_colliders[i].transform.position, 0.3f);
        }
        Gizmos.color = Color.green;
        foreach (var go in _objectsInSight)
        {
            Gizmos.DrawSphere(go.transform.position, 0.5f);
        }
    }
}
