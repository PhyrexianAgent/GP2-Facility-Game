using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLooking : State
{
    private const float TOLERANCE = 0.005f; // For testing if looking at correct direction (gives some range in end rotation)
    private float turnRate;
    private float lookDuration;
    private Transform[] lookPoints;
    private int currentLookIndex;
    private Transform cameraHead;
    private MonoBehaviour script;
    private bool looking = false;
    private Coroutine lookingDelay;
    private Vector3 lastLookDir;
    public CameraLooking(GameObject agent, float turnRate, Transform[] lookPoints, float lookDuration, Transform cameraHead) : base(agent){
        this.turnRate = turnRate;
        this.lookPoints = lookPoints;
        this.lookDuration = lookDuration;
        this.cameraHead = cameraHead;
        script = agent.GetComponent<MonoBehaviour>();
    }
    public override void OnEnter(){
        looking = false;
        currentLookIndex = GetNearestLookPointIndex();
        if (lookPoints.Length == 0) Debug.LogWarning("Missing Lookpoints on Advanced Turret or Camera");
    }
    public override void OnExit(){
        if (lookingDelay != null) script.StopCoroutine(lookingDelay);
    }
    public override void Update(){
        if (!looking && lookPoints.Length > 0) RotateToCurrentPoint();
    }
    private void RotateToCurrentPoint(){ // Will be kept like this so old dir can be collected
        Vector3 diff = lookPoints[currentLookIndex].position - cameraHead.position;
        Vector3 newDirection = Vector3.RotateTowards(cameraHead.forward, diff, turnRate * Time.deltaTime, 0.0f);
        cameraHead.rotation = Quaternion.LookRotation(newDirection);
        if (Vector3.Distance(newDirection, lastLookDir) <= TOLERANCE) 
            lookingDelay = script.StartCoroutine(LookAtPoint());
        lastLookDir = newDirection;
    }
    private IEnumerator LookAtPoint(){
        looking = true;
        yield return new WaitForSeconds(lookDuration);
        looking = false;
        AddLookIndex();
    }

    private void AddLookIndex(){
        currentLookIndex++;
        if (currentLookIndex >= lookPoints.Length) currentLookIndex = 0;
    }

    private int GetNearestLookPointIndex(){
        int lowestIndex = 0;
        float lowestDotDiff = 1;
        for(int i=1; i<lookPoints.Length; i++){
            Vector3 diff = (lookPoints[i].position - cameraHead.position).normalized;
            float dotDiff = 1 - Vector3.Dot(diff, cameraHead.forward);
            if (dotDiff < lowestDotDiff){
                lowestIndex = i;
                lowestDotDiff = dotDiff;
            }
        }
        return lowestIndex;
    }
}
