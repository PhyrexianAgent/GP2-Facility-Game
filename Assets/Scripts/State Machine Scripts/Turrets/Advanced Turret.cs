using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTurret : StateMachine
{
    public Vector3 CurrentTarget {private get; set;}
    public bool ActiveFromCamera {private get; set;}
    [SerializeField] private Transform[] lookPoints;
    [SerializeField, Min(0)] private float idleTurnRate, alertedTurnRate, lookDuration;
    [SerializeField] private GameObject turretHead;
    private GameObject controllingCamera;
    void Start()
    {
        InitializeStates();
    }

    private void InitializeStates(){
        CameraLooking idle = new CameraLooking(gameObject, idleTurnRate, lookPoints, lookDuration, turretHead.transform);
        TurretActive activeFromCam = new TurretActive(gameObject, TurnToCurrentTarget);
        TurretLookNearCover lookingNearCover = new TurretLookNearCover(gameObject, turretHead.transform, alertedTurnRate, GetObstacle);

        AddNode(idle, true);
        AddNode(activeFromCam);
        AddNode(lookingNearCover);

        AddTransition(idle, activeFromCam, new Predicate(() => ActiveFromCamera));
        AddTransition(activeFromCam, lookingNearCover, new Predicate(() => !ActiveFromCamera));
        //AddTransition
    }
    private bool RotateToAngle(Vector3 dir){
        turretHead.transform.rotation = Quaternion.LookRotation(dir);
        return true;
    }
    private Vector3 GetCurrentTarget() => CurrentTarget;
    private bool TurnToCurrentTarget(){
        turretHead.transform.rotation = GameManager.GetRotationToPointOverTime(turretHead.transform, CurrentTarget, alertedTurnRate);
        return true;
    }
    private GameObject GetObstacle() => GameManager.GetObjectBetweenPlayerAndTarget(controllingCamera.transform);
    public void SetControllingCamera(GameObject camera) => controllingCamera = camera;
}
