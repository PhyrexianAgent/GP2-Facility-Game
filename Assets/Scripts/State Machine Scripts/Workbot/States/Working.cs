using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Working : EventListenerState<Sound>
{
    public Working(GameObject agent, UnityEvent<Sound> soundEvent) : base(agent, soundEvent){

    }

    protected override void EventCallMethod(Sound sound){
        if (sound.CanHearSound(agent.transform.position)){
            TransitionFromEvent = true;
            Debug.Log("heard sound");
        }
    }
}
