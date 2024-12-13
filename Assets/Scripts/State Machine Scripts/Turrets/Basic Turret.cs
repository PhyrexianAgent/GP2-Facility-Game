using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicTurret : StateMachine
{
    public Vector3 CurrentTarget {private get; set;}
    public bool IsActive {private get; set;}
    [SerializeField] private GameObject turretArm, turretBase;
    [SerializeField, Min(0)] private float turnRate;
    void Start()
    {
        InitializeStates();
    }

    private void InitializeStates(){
        TurretIdle idle = new TurretIdle(turretArm);
        TurretActive active = new TurretActive(turretArm, TurnToTarget);

        AddNode(idle, true);
        AddNode(active);

        AddTransition(idle, active, new Predicate(() => IsActive));
        AddTransition(active, idle, new Predicate(() => !IsActive));
    }

    private bool TurnToDirection(Vector3 direction){
        turretArm.transform.rotation = Quaternion.LookRotation(direction);
        return true;
    }
    private Vector3 GetCurrentTarget() => CurrentTarget;

    private bool TurnToTarget(){
        turretArm.transform.rotation = GameManager.GetRotationToPointOverTime(turretArm.transform, CurrentTarget, turnRate);
        return true;
    }
}
