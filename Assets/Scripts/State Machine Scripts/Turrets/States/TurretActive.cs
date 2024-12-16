using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurretActive : State
{
    private Func<bool> updateRotationFunc; // Using this so I can have a different State Machine script use this state later
    private Func<Vector3> getTargetPosFunc;
    private float turnRate;
    private Vector3 currentTargetPos;
    private Vector3 lastDirection;
    private Animator anim;
    public TurretActive(GameObject agent, Func<bool> updateRotationFunc) : base(agent){
        this.updateRotationFunc = updateRotationFunc;
        this.getTargetPosFunc = getTargetPosFunc;
        this.turnRate = turnRate;
        anim = agent.GetComponent<Animator>();
    }
    public override void Update(){
        updateRotationFunc();
        if (anim != null){
            anim.Play("Spin Up");
        } 
    }
}
