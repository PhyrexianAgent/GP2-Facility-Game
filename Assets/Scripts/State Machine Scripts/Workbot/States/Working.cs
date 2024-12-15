using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Working : EventListenerState<Sound>
{
    private Quaternion startRotation;
    private float turnRate;
    private bool turningToStart = false;
    public Working(GameObject agent, UnityEvent<Sound> soundEvent, float turnRate) : base(agent, soundEvent){
        startRotation = agent.transform.rotation;
        this.turnRate = turnRate;
    }
    public override void OnEnter(){
        base.OnEnter();
        turningToStart = agent.transform.rotation != startRotation;
        //Debug.Log(turningToStart);
    }
    public override void Update(){
        if (turningToStart && agent.transform.rotation != startRotation){
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, startRotation, turnRate * Time.deltaTime * 30);
        }
    }

    protected override void EventCallMethod(Sound sound){
        if (sound.CanHearSound(agent.transform.position)){
            TransitionFromEvent = true;
        }
    }
}
