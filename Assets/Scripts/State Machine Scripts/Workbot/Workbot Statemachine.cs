using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class WorkbotStatemachine : StateMachine
{
    [SerializeField, Min(0)] private float turnRate, lookDuration;
    [SerializeField] private Transform fleePoint;
    [SerializeField] private ConeDetector visionCone;
    [SerializeField, Min(0)] private float fleeSpeed = 5;
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
        Fleeing fleeing = new Fleeing(gameObject, fleePoint.position, fleeSpeed);

        AddNode(working, true);
        AddNode(looking);
        AddNode(fleeing);

        AddTransition(working, looking, new Predicate(() => working.TransitionFromEvent), SetInitialSearchDirection);
        AddTransition(looking, fleeing, new Predicate(() => visionCone.PlayerInSpotlight(GameManager.GetPlayerTransform()) && GameManager.PlayerInView(visionCone.transform.position)));
    }

    private bool SetInitialSearchDirection(IState state){
        StateNode node = GetNode(state);
        LookingTowardsSound looking = (LookingTowardsSound)node.State;
        looking.SetInitialLookPoint(lastHeardSound.Location);
        return true;
    }

    public void HeardSound(Sound sound) => lastHeardSound = sound;
    public void KeyPressed(){
        GameManager.CollectWorkBot();
        Destroy(gameObject);
    }
}
