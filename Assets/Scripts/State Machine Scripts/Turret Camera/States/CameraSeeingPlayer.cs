using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeeingPlayer : State
{
    private Transform cameraHead;
    private float turnRate;

    public CameraSeeingPlayer(GameObject agent, Transform cameraHead, float turnRate) : base(agent){
        this.cameraHead = cameraHead;
        this.turnRate = turnRate;
    }
    public override void Update(){
        RotateToPlayer();
    }
    private void RotateToPlayer(){
        Vector3 diff = GameManager.GetPlayerTransform().position - cameraHead.position;
        Vector3 newDirection = Vector3.RotateTowards(cameraHead.forward, diff, turnRate * Time.deltaTime, 0.0f);
        cameraHead.rotation = Quaternion.LookRotation(newDirection);
    }
}
