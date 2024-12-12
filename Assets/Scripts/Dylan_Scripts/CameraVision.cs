using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVision : Perception
{
    [SerializeField] private MeshCollider _collider;
    [SerializeField] private float _distance = 20f;
    [SerializeField] private float _angle = 20f;
    [SerializeField] private int _scanFrequency = 32;
    [SerializeField] private LayerMask _trackableLayers;
    [SerializeField] private LayerMask _occlusionLayers;
    [SerializeField] private List<GameObject> _objectsInSight = new();

    private List<Collider> _colliders = new();
    private int _count;
    private float _scanInterval;
    private float _scanTimer;

    public bool trackableIsInSight;
    private bool _delayFinished;
    private float _delayLimit = 4f;
    private float _delayTimer;

    void Start()
    {
        _collider = GetComponent<MeshCollider>();
        gameObject.transform.localScale = new Vector3(_angle * 3f, _angle * 3f, _distance * 9.375f);
        //_collider.m_distance = _distance;
        //_collider.m_angle = _angle;
    }

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
        _count = _colliders.Count;

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

        // Check if sight is interupted by an object
        //Debug.DrawLine(origin, destination, Color.red, 1f);
        if (Physics.Linecast(origin, destination, _occlusionLayers))
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        if (!VisualDebug) { return; }
        // Draw Cone Collider
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger Entered");
            _colliders.Add(obj);
        }
    }

    private void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger Exited");
            _colliders.Remove(obj.GetComponent<Collider>());
        }
    }
}
