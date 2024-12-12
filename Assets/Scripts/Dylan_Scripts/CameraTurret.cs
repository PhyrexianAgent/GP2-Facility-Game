using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurret : StateMachine
{
    //[SerializeField] private Animator _animator; #using transform operations for now
    [SerializeField] private GameObject _player;
    [SerializeField] private CameraVision _vision;
    [SerializeField] private GameObject[] _turrets;
    [SerializeField] private float _patrolAngle = 30f;
    [SerializeField] private float _patrolSpeed = 1f;
    [SerializeField] private float _aimSpeed = 3f;

    [SerializeField] private float _damage = 1f;

    private StateMachine _stateMachine;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _vision = GetComponentInChildren<CameraVision>();

    }

    private void Start()
    {
        GameObject cameraObject = FindDescendant(transform, "Camera");
        var patrolState = new CameraPatrolState(_turrets, _player, cameraObject, _patrolAngle, _patrolSpeed);
        var alertState = new CameraAlertState(_turrets, _player, cameraObject, _aimSpeed, _damage);
        AddNode(patrolState, true);
        AddNode(alertState);

        AddTransition(patrolState, alertState, new Predicate(() => _vision.trackableIsInSight && Vector3.Distance(_player.transform.position, transform.position) <= 10f));
        AddTransition(alertState, patrolState, new Predicate(() => Vector3.Distance(_player.transform.position, transform.position) > 10f || !_vision.trackableIsInSight));

        //SetCurrentState(patrolState);
    }

    GameObject FindDescendant(Transform parent, string target)
    {
        // Check if current descendant matches
        if (parent.gameObject.name == target)
        {
            return parent.gameObject;
        }

        // Search through all descendants
        foreach (Transform child in parent)
        {
            GameObject found = FindDescendant(child, target);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
}
