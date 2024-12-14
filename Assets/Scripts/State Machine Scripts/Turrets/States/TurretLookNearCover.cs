using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurretLookNearCover : State
{
    public bool DoneLooking {get; private set;} = false;
    private Transform head;
    private float turnRate;
    private Func<GameObject> getObstacle;
    private GameObject playerCover;
    private float pastObstacleDelay;
    private float lookDuration;
    private bool inDelay = false;
    private Coroutine pastObstacleWait;
    private Coroutine lookWait;
    private MonoBehaviour script;
    private int lookCount = 0;
    private bool isLooking = false;
    private Directions lookDir;
    enum Directions{
        right,
        left
    }
    public TurretLookNearCover(GameObject agent, Transform head, float turnRate, Func<GameObject> getObstacle, float pastObstacleDelay, float lookDuration) : base(agent){
        this.head = head;
        this.turnRate = turnRate;
        this.getObstacle = getObstacle;
        this.pastObstacleDelay = pastObstacleDelay;
        this.lookDuration = lookDuration;
        script = agent.GetComponent<MonoBehaviour>();
    }
    public override void OnEnter(){
        playerCover = getObstacle();
        Debug.Log("entered");
    }
    public override void OnExit(){
        if (pastObstacleWait != null) script.StopCoroutine(pastObstacleWait);
        if (lookWait != null) script.StopCoroutine(lookWait);
        DoneLooking = false;
        inDelay = false;
        isLooking = false;
        lookCount = 0;
    }
    public override void Update(){
        if (!isLooking){
            RotateInDirection();
            if (pastObstacleWait == null && lookWait == null) CheckForPastObstacle();
        } 
    }

    private void RotateInDirection(){
        Vector3 dir = Vector3.RotateTowards(head.forward, lookDir == Directions.right ? head.right : -head.right, turnRate * Time.deltaTime, 0.0f);
        head.rotation = Quaternion.LookRotation(dir);
    }
    private void CheckForPastObstacle(){
        GameObject objectFromRay = GameManager.GetObjectFromRay(head.position, head.forward);
        if (objectFromRay != playerCover) pastObstacleWait = script.StartCoroutine(PastObstacleStopDelay());
    }

    private IEnumerator PastObstacleStopDelay(){
        inDelay = true;
        yield return new WaitForSeconds(pastObstacleDelay);
        lookWait = script.StartCoroutine(LookDelay());
    }
    private IEnumerator LookDelay(){
        isLooking = true;
        yield return new WaitForSeconds(lookDuration);
        isLooking = false;
        ChangeLookDirection();
    }
    private void ChangeLookDirection(){
        lookCount++;
        DoneLooking = lookCount >= 2;
        if (!DoneLooking){
            lookDir = lookDir == Directions.left ? Directions.right : Directions.left;
        }
    }

}
