using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class LookingBehindCover : State
{
    private const string RUN_ANIM = "RunWithGun";
    private const string IDLE_ANIM = "IdleArmed";
    private float searchTime;
    private Vector3 lookingDestination;
    private Coroutine lookingTimer;
    private bool doneLooking = false;
    private bool foundDistance = false; // This is here because on first update, remaining distance is always 0
    private float searchCoverSpeed;

    public LookingBehindCover(GameObject agent, float searchTime, float searchCoverSpeed) : base(agent){
        this.searchTime = searchTime;
        this.searchCoverSpeed = searchCoverSpeed;
    }

    public override void OnEnter(){
        navAgent.SetDestination(lookingDestination);
        navAgent.speed = searchCoverSpeed;
        Debug.Log("in enter");
        animator.Play(RUN_ANIM, 0);
    }
    public override void Update(){
        //Debug.Log(navAgent.remainingDistance);
        if (foundDistance && navAgent.remainingDistance == 0 && lookingTimer == null && !doneLooking){
            animator.Play(IDLE_ANIM);
            lookingTimer = BigBotStateMachine.instance.StartCoroutine(LookTimer());
        }
        if (navAgent.remainingDistance > 0) // Fixes issue where transition to holstering will always occur instantly as remaining distance is always 0 on first update
            foundDistance = true;
    }
    public override void OnExit(){
        if (lookingTimer != null){
            BigBotStateMachine.instance.StopCoroutine(lookingTimer);
        }
        doneLooking = false;
        foundDistance = false;
    }
    public bool DoneLooking() => doneLooking;
    public void SetLookingDestination(Vector3 point){
        lookingDestination = point;
    }

    private IEnumerator LookTimer(){
        yield return new WaitForSeconds(searchTime);
        doneLooking = true;
    }
}
