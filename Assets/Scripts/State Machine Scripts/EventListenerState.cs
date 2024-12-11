using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerState<T> : State
{
    protected UnityEvent<T> unityEvent;
    public bool TransitionFromEvent {get; protected set;}
    protected EventListenerState(GameObject agent, UnityEvent<T> unityEvent) : base(agent){
        this.unityEvent = unityEvent;
    }

    public override void OnEnter() { // Note you will need to call this base method otherwise will not add method to list of methods for the unity event
        TransitionFromEvent = false;
        unityEvent.AddListener(EventCallMethod);
    } 
    public override void OnExit() => unityEvent.RemoveListener(EventCallMethod); // Note you will need to call this base method otherwise will not add method to list of methods for the unity event

    protected virtual void EventCallMethod(T value) => Debug.Log("calling base method");
}
