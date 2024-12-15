using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fleeing : State
{
    private Vector3 fleePoint;
    private float fleeSpeed;
    private NavMeshAgent navAgent;
    public Fleeing(GameObject agent, Vector3 fleePoint, float fleeSpeed) : base(agent){
        this.fleePoint = fleePoint;
        this.fleeSpeed = fleeSpeed;
        navAgent = agent.GetComponent<NavMeshAgent>();
    }
    public override void OnEnter(){
        navAgent.SetDestination(fleePoint);
        navAgent.speed = fleeSpeed;
    }
    public override void Update(){
        if (navAgent.remainingDistance <= 0.15f) MonoBehaviour.Destroy(agent);
    }
}
