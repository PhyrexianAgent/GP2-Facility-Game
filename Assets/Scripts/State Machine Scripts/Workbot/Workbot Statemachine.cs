using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class WorkbotStatemachine : StateMachine
{
    [SerializeField, Min(0)] private float turnRate, lookDuration;
    private SoundListener soundListener;
    private NavMeshAgent navAgent;
    private Sound lastHeardSound;
    //private Working test;

    void Awake(){
        soundListener = GetComponent<SoundListener>();
        navAgent = GetComponent<NavMeshAgent>();

    }
    void Start()
    {
        UnityEvent<Sound> unityEvent = soundListener.GetUnityEvent();
        Working working = new Working(gameObject, unityEvent);
        LookingTowardsSound looking = new LookingTowardsSound(gameObject, unityEvent, turnRate, lookDuration);

        AddNode(working, true);
        AddNode(looking);

        AddTransition(working, looking, new Predicate(() => working.TransitionFromEvent), SetInitialSearchDirection);
    }

    private bool SetInitialSearchDirection(IState state){
        StateNode node = GetNode(state);
        LookingTowardsSound looking = (LookingTowardsSound)node.State;
        looking.SetInitialLookPoint(lastHeardSound.Location);
        return true;
    }

    public void HeardSound(Sound sound) => lastHeardSound = sound;
}
