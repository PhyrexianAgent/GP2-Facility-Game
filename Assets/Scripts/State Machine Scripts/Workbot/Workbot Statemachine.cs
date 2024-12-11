using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class WorkbotStatemachine : StateMachine
{
    [SerializeField] private Transform testMovePoint;
    private SoundListener soundListener;
    private NavMeshAgent navAgent;
    //[SerializeField] private UnityEvent<Sound> soundEvent;
    void Awake(){
        soundListener = GetComponent<SoundListener>();
        navAgent = GetComponent<NavMeshAgent>();

    }
    void Start()
    {
        Working working = new Working(gameObject, soundListener.GetUnityEvent());

        AddNode(working, true);

        navAgent.SetDestination(testMovePoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void HeardSound(Sound sound)
}
