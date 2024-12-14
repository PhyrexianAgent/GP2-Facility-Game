using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : State
{
    private const string WALK_ANIMATION = "Walk";
    private Transform[] patrolPoints; // Do note does assume points are ordered in path big bot will travel.
    private int currentPointIndex = 0;
    private float patrolSpeed;
    public bool CanAttack {get; private set;}
    private Coroutine attackCooldown;

    public Patrolling(GameObject agent, Transform[] patrolPoints, float patrolSpeed, bool canAttack) : base(agent){
        this.patrolPoints = patrolPoints;
        this.patrolSpeed = patrolSpeed;
        CanAttack = canAttack;
    }
    public override void OnEnter(){
        currentPointIndex = GetNearestPatrolPointIndex();
        navAgent.SetDestination(patrolPoints[currentPointIndex].position);
        navAgent.speed = patrolSpeed;
        animator.Play(WALK_ANIMATION, 0);
    }
    public override void Update(){
        if (navAgent.remainingDistance <= 0.15f){
            Debug.Log("is 0");
            AddPointIndex();
            navAgent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }
    public override void OnExit(){
        if (attackCooldown != null)
            BigBotStateMachine.instance.StopCoroutine(attackCooldown);
    }
    public void SaveAttackCooldown(Coroutine coroutine){
        attackCooldown = coroutine;
    }
    public IEnumerator AttackCooldown(){ // Only for testing and will force bot not to attack for 5 seconds
        CanAttack = false;
        yield return new WaitForSeconds(5);
        CanAttack = true;
    }

    private void AddPointIndex(){
        currentPointIndex++;
        if (currentPointIndex >= patrolPoints.Length){
            currentPointIndex = 0;
        }
    }
    private int GetNearestPatrolPointIndex(){
        int currentLowestDistanceIndex = 0;
        float lastLowestDistance = Vector3.Distance(agent.transform.position, patrolPoints[0].position);
        for (int i=1; i<patrolPoints.Length; i++){
            float distance = Vector3.Distance(agent.transform.position, patrolPoints[i].position);
            if (distance < lastLowestDistance){
                currentLowestDistanceIndex = i;
                lastLowestDistance = distance;
            }
        }
        return currentLowestDistanceIndex;
    }
}
