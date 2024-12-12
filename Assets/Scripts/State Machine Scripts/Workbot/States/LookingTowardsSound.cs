using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookingTowardsSound : EventListenerState<Sound>
{
    public bool DoneLooking {get; private set;} = false;
    private Vector3 lookPoint;
    private float turnRate;
    private float lookDuration;
    private bool atEndRotation = false;
    private WorkbotStatemachine coroutineRunner; // Note this makes this state require the Workbot state machine script
    private Coroutine lookCoroutine;
    private bool lookingAtPoint = false;
    public LookingTowardsSound(GameObject agent, UnityEvent<Sound> unityEvent, float turnRate, float lookDuration) : base(agent, unityEvent){
        this.turnRate = turnRate;
        this.lookDuration = lookDuration;
        coroutineRunner = agent.GetComponent<WorkbotStatemachine>();
    }
    public override void OnEnter(){
        base.OnEnter();
        DoneLooking = false;
    }
    public override void Update(){
        if (!DoneLooking && !lookingAtPoint) LookToLookPoint();
    }

    protected override void EventCallMethod(Sound sound){
        //currentLookRotation = Quaternion.LookRotation(sound.Location);//sound.Location;
        lookPoint = sound.Location;
        lookingAtPoint = false;
        atEndRotation = false;
        if (lookCoroutine != null) coroutineRunner.StopCoroutine(lookCoroutine);
    }
    private void LookToLookPoint(){
        Vector3 diff = lookPoint - new Vector3(agent.transform.position.x, lookPoint.y, agent.transform.position.z);
        Vector3 newDirection = Vector3.RotateTowards(agent.transform.forward, diff, turnRate * Time.deltaTime, 0.0f);
        bool finishedRotation = agent.transform.forward == newDirection;
        agent.transform.rotation = Quaternion.LookRotation(newDirection);
        if (finishedRotation) lookCoroutine = coroutineRunner.StartCoroutine(LookDelay());
        //agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, currentLookRotation, turnRate * Time.deltaTime);
        //if (agent.transform.rotation == diff) lookCoroutine = coroutineRunner.StartCoroutine(LookDelay());
    }
    /*private bool IsLookingAtPoint(){
        //Vector3 testPos = new Vector3(agent.transform.position.x, lookPoint.y, agent.transform.position.z); // Done to get y pos the same as look point
        //Vector3 diff = lookPoint - new Vector3
        float dot = Vector3.Dot((lookPoint - testPos).normalized, agent.transform.forward);
        return dot >= 0.98f;
    }*/
    private IEnumerator LookDelay(){
        lookingAtPoint = true;
        Debug.Log("Strated Looking");
        yield return new WaitForSeconds(lookDuration);
        DoneLooking = true;
        Debug.Log("Done Looking");
    }
    public void SetInitialLookPoint(Vector3 point){
        //currentLookRotation = Quaternion.LookRotation(point);
        lookPoint = point;
    }
}
