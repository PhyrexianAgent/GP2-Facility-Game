using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLostPlayer : State
{
    public bool FinishedDelay {get; private set;}
    private float lostWaitDuration;
    private TurretCamera script;
    private Coroutine waitCoroutine;
    public CameraLostPlayer(GameObject agent, float lostWaitDuration) : base(agent){
        this.lostWaitDuration = lostWaitDuration;
        script = agent.GetComponent<TurretCamera>();
    }
    public override void OnEnter(){
        FinishedDelay = false;
        waitCoroutine = script.StartCoroutine(LostPlayerDelay());
    }
    public override void OnExit(){
        if (waitCoroutine != null) script.StopCoroutine(waitCoroutine);
    }

    private IEnumerator LostPlayerDelay(){
        yield return new WaitForSeconds(lostWaitDuration);
        FinishedDelay = true;
    }
}
