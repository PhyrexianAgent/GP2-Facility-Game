using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurretLookNearCover : State
{
    private Transform head;
    private float turnRate;
    private Func<GameObject> getObstacle;
    public TurretLookNearCover(GameObject agent, Transform head, float turnRate, Func<GameObject> getObstacle) : base(agent){
        this.head = head;
        this.turnRate = turnRate;
        this.getObstacle = getObstacle;
    }
    public override void OnEnter(){
        Debug.Log(getObstacle().name);
    }
}
