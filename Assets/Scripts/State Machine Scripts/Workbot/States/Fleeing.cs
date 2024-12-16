using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fleeing : State
{
    private Vector3 fleePoint;
    private float fleeSpeed;
    private NavMeshAgent navAgent;

    private AudioSource audioSource;
    private AudioClip audioClip;
    public Fleeing(GameObject agent, Vector3 fleePoint, float fleeSpeed, AudioSource audioSource, AudioClip audioClip) : base(agent){
        this.fleePoint = fleePoint;
        this.fleeSpeed = fleeSpeed;
        navAgent = agent.GetComponent<NavMeshAgent>();

        this.audioSource = audioSource;
        this.audioClip = audioClip;
    }
    public override void OnEnter(){
        navAgent.SetDestination(fleePoint);
        navAgent.speed = fleeSpeed;

        if (audioSource.isPlaying) audioSource.Stop(); 
        audioSource.loop = false;
        audioSource.clip = audioClip;
        audioSource.volume = 0.7f;
        audioSource.Play();
    }
    public override void Update(){
        if (navAgent.remainingDistance <= 0.15f) MonoBehaviour.Destroy(agent);
    }

    public override void OnExit()
    {
        audioSource.Stop();
    }
}
