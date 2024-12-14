using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTurret : StateMachine
{
    public Vector3 CurrentTarget {private get; set;}
    public bool ActiveFromCamera {private get; set;}
    [SerializeField] private Transform[] lookPoints;
    [SerializeField, Min(0)] private float idleTurnRate, alertedTurnRate, pastObstacleDelay, lookDuration;
    [SerializeField] private GameObject turretHead;
    [SerializeField] private Transform lookPoint; // for where 'laser' will exit from 
    private GameObject controllingCamera;
    void Start()
    {
        InitializeStates();
    }

    private void InitializeStates(){
        CameraLooking idle = new CameraLooking(gameObject, idleTurnRate, lookPoints, lookDuration, turretHead.transform);
        TurretActive activeFromCam = new TurretActive(gameObject, TurnToCurrentTarget);
        AdvancedTurretSawPlayer sawPlayer = new AdvancedTurretSawPlayer(gameObject, turretHead, lookDuration, lookPoint);

        AddNode(idle, true);
        AddNode(activeFromCam);
        AddNode(sawPlayer);

        AddTransition(idle, activeFromCam, new Predicate(() => ActiveFromCamera));
        AddTransition(activeFromCam, sawPlayer, new Predicate(() => GameManager.PlayerInView(lookPoint.position, lookPoint.forward)));
        AddTransition(idle, sawPlayer, new Predicate(() => GameManager.PlayerInView(lookPoint.position, lookPoint.forward)));
        AddTransition(sawPlayer, idle, new Predicate(() => sawPlayer.DoneLooking));
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
