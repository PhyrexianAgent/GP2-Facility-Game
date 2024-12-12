using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IndependantTurret : StateMachine
{
    //[SerializeField] private Animator _animator; #using transform operations for now
    [SerializeField] private TurretVision _vision;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _patrolSpeed = 1f;
    [SerializeField] private float _aimSpeed = 3f;
    [SerializeField] private VisionRange _range;

    [SerializeField] private float _damage = 1f;

    private StateMachine _stateMachine;

    private void Awake()
    {
        _vision = GetComponentInChildren<TurretVision>();
        _audioSource = GetComponent<AudioSource>();
        _range = GetComponentInChildren<VisionRange>();

    }

    private void Start()
    {
        var patrolState = new TurretPatrolState(gameObject, _range, _patrolSpeed);
        var attackState = new TurretAttackState(gameObject, _range, _aimSpeed, _audioSource, _damage);
        AddNode(patrolState, true);
        AddNode(attackState);

        AddTransition(patrolState, attackState, new Predicate(() => _vision.trackableIsInSight));
        AddTransition(attackState, patrolState, new Predicate(() => Vector3.Distance(GameManager.GetPlayerTransform().position, transform.position) > 10f || !_vision.trackableIsInSight));

        //SetCurrentState(patrolState);
    }
}
