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

    private AudioSource audioSource;
    [SerializeField] AudioClip idleClip;
    [SerializeField] AudioClip fleeClip;
    

    void Awake(){
        soundListener = GetComponent<SoundListener>();
        navAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        UnityEvent<Sound> unityEvent = soundListener.GetUnityEvent();
        Working working = new Working(gameObject, unityEvent, turnRate, audioSource, idleClip);
        LookingTowardsSound looking = new LookingTowardsSound(gameObject, unityEvent, turnRate, lookDuration, audioSource, idleClip);
        Fleeing fleeing = new Fleeing(gameObject, fleePoint.position, fleeSpeed, audioSource, fleeClip);

        AddNode(working, true);
        AddNode(looking);
        AddNode(fleeing);

        AddTransition(working, looking, new Predicate(() => working.TransitionFromEvent), SetInitialSearchDirection);
        AddTransition(looking, fleeing, new Predicate(() => visionCone.PlayerInSpotlight(GameManager.GetPlayerTransform()) && GameManager.PlayerInView(visionCone.transform.position)));
        AddTransition(looking, working, new Predicate(() => looking.DoneLooking));
    }

    private bool SetInitialSearchDirection(IState state){
        StateNode node = GetNode(state);
        LookingTowardsSound looking = (LookingTowardsSound)node.State;
        looking.SetInitialLookPoint(lastHeardSound.Location);
        return true;
    }

    public void HeardSound(Sound sound){
        lastHeardSound = sound;
    } //=> lastHeardSound = sound;
    public void KeyPressed(){
        GameManager.CollectWorkBot();
        Destroy(gameObject);
    }
}
