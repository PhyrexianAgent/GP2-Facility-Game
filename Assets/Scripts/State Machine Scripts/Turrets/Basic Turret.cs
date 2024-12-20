using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicTurret : StateMachine
{
    public Vector3 CurrentTarget {private get; set;}
    public bool IsActive {private get; set;}
    [SerializeField] private GameObject turretArm;
    [SerializeField, Min(0)] private float turnRate;
    private TurretDamageController damageController;
    void Awake(){
        damageController = GetComponent<TurretDamageController>();
    }
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
        damageController.SetDamageActive(GameManager.PlayerInView(turretArm.transform.position, turretArm.transform.forward));
        //turretParticles.SetActive(GameManager.PlayerInView(turretArm.transform.position, turretArm.transform.forward));
        return true;
    }
}
