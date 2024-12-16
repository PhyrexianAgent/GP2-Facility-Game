using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Working : EventListenerState<Sound>
{
    private Quaternion startRotation;
    private float turnRate;
    private bool turningToStart = false;
    private AudioSource audioSource;
    private AudioClip audioClip;
    public Working(GameObject agent, UnityEvent<Sound> soundEvent, float turnRate, AudioSource audioSource, AudioClip audioClip) : base(agent, soundEvent){
        startRotation = agent.transform.rotation;
        this.turnRate = turnRate;

        this.audioSource = audioSource;
        this.audioClip = audioClip;
    }
    public override void OnEnter(){
        base.OnEnter();
        turningToStart = agent.transform.rotation != startRotation;

        audioSource.loop = true;
        audioSource.volume = 0.1f;
        if (audioSource.clip != audioClip) audioSource.clip = audioClip;
        if (!audioSource.isPlaying) audioSource.Play();

        //Debug.Log(turningToStart);
    }
    public override void Update(){
        if (turningToStart && agent.transform.rotation != startRotation){
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, startRotation, turnRate * Time.deltaTime * 30);
        }
    }

    public override void OnExit()
    {
        //audioSource.Stop();
    }

    protected override void EventCallMethod(Sound sound){
        if (sound.CanHearSound(agent.transform.position)){
            TransitionFromEvent = true;
        }
    }
}
