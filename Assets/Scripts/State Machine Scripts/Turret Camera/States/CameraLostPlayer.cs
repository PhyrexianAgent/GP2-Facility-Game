using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLostPlayer : State
{
    public bool FinishedDelay {get; private set;}
    private float lostWaitDuration;
    private TurretCamera script;
    private Coroutine waitCoroutine;
    private BasicTurret[] basicTurrets;
    private AdvancedTurret[] advancedTurrets;
    public CameraLostPlayer(GameObject agent, float lostWaitDuration, BasicTurret[] basicTurrets, AdvancedTurret[] advancedTurrets) : base(agent){
        this.lostWaitDuration = lostWaitDuration;
        script = agent.GetComponent<TurretCamera>();
        this.basicTurrets = basicTurrets;
        this.advancedTurrets = advancedTurrets;
    }
    public override void OnEnter(){
        FinishedDelay = false;
        waitCoroutine = script.StartCoroutine(LostPlayerDelay());
        foreach(BasicTurret turret in basicTurrets) turret.IsActive = false;
        foreach(AdvancedTurret turret in advancedTurrets) turret.ActiveFromCamera = false;
    }
    public override void OnExit(){
        if (waitCoroutine != null) script.StopCoroutine(waitCoroutine);
    }

    private IEnumerator LostPlayerDelay(){
        yield return new WaitForSeconds(lostWaitDuration);
        FinishedDelay = true;
    }
}
