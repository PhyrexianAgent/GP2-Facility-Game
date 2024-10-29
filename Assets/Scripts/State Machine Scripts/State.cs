using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State : IState
{
    protected readonly NavMeshAgent navAgent;
    protected readonly GameObject agent;
    protected readonly Animator animator;

    protected State(GameObject agent){
        this.agent = agent;
        this.animator = agent.GetComponent<Animator>();
        this.navAgent = agent.GetComponent<NavMeshAgent>();
    }
    public virtual void OnEnter(){ Debug.Log("wrong area");}
    public virtual void OnExit(){}
    public virtual void Update(){}
    public virtual void FixedUpdate(){}
}
