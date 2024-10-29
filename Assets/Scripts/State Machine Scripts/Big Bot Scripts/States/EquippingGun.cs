using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquippingGun : State
{
    private const string DRAW_CANNON_ANIM = "DrawCannon";
    private bool playerBehindCover = false;
    private Transform player;
    private Func<bool> canSeePlayer;

    public bool FinishedAnimation() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0);
    public bool CanSeePlayer() => !playerBehindCover;
    public EquippingGun(GameObject agent, Transform player, Func<bool> canSeePlayer) : base(agent){
        this.player = player;
        this.canSeePlayer = canSeePlayer;
    }
    public override void OnEnter(){
        //playerBehindCover = false;
        navAgent.SetDestination(BigBotStateMachine.instance.transform.position);
        animator.Play(DRAW_CANNON_ANIM, 0);
    }
    public override void Update(){
        if (!playerBehindCover && canSeePlayer()){
            agent.transform.LookAt(player.position);
        }
        else if (!canSeePlayer()){
            playerBehindCover = true;
        }
    }
    public override void OnExit(){
        playerBehindCover = false;
    }
}
