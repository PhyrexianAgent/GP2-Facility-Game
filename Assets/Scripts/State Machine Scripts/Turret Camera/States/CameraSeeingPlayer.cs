using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeeingPlayer : State
{
    private Transform cameraHead;
    private float turnRate;
    private BasicTurret[] basicTurrets;

    public CameraSeeingPlayer(GameObject agent, Transform cameraHead, float turnRate, BasicTurret[] basicTurrets) : base(agent){
        this.cameraHead = cameraHead;
        this.turnRate = turnRate;
        this.basicTurrets = basicTurrets;
    }
    public override void OnEnter(){
        foreach(BasicTurret turret in basicTurrets) turret.IsActive = true;
    }
    public override void Update(){
        RotateToPlayer();
        SetTurretsTarget();
    }
    private void RotateToPlayer(){
        Vector3 diff = GameManager.GetPlayerTransform().position - cameraHead.position;
        Vector3 newDirection = Vector3.RotateTowards(cameraHead.forward, diff, turnRate * Time.deltaTime, 0.0f);
        cameraHead.rotation = Quaternion.LookRotation(newDirection);
    }
    private void SetTurretsTarget(){
        foreach(BasicTurret turret in basicTurrets) turret.CurrentTarget = GameManager.GetPlayerTransform().position;
    }
}
