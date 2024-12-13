using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurretActive : State
{
    private Func<Vector3, bool> updateRotationFunc; // Using this so I can have a different State Machine script use this state later
    private Func<Vector3> getTargetPosFunc;
    private float turnRate;
    private Vector3 currentTargetPos;
    private Vector3 lastDirection;
    public TurretActive(GameObject agent, Func<Vector3, bool> updateRotationFunc, Func<Vector3> getTargetPosFunc, float turnRate) : base(agent){
        this.updateRotationFunc = updateRotationFunc;
        this.getTargetPosFunc = getTargetPosFunc;
        this.turnRate = turnRate;
    }
    public override void Update(){
        currentTargetPos = getTargetPosFunc();
        Debug.Log(currentTargetPos);
        PointToTarget();

    }
    private void PointToTarget(){
        Vector3 diff = agent.transform.position - currentTargetPos;
        Vector3 newDir = Vector3.RotateTowards(agent.transform.forward, diff, turnRate * Time.deltaTime, 0.0f);
        updateRotationFunc(newDir);
    }
}
