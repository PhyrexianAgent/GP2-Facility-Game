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

    public override void OnEnter() {
        TransitionFromEvent = false;
        unityEvent.AddListener(EventCallMethod);
    } 
    public virtual void OnExit() => unityEvent.RemoveListener(EventCallMethod);

    protected virtual void EventCallMethod(T value) => Debug.Log("calling base method");
}
