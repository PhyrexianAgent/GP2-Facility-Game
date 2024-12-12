using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCamera : StateMachine
{
    [SerializeField] Transform cameraHead, cameraBase;
    [SerializeField] Transform[] lookPoints; // Camera will look between a bunch of points in this list (makes designing where camera looks easier)
    [SerializeField, Min(0)] float baseTurnrate, seenPlayerTurnRate, lookDuration;
    [SerializeField] private ConeDetector coneDetector;
    [SerializeField] private LayerMask playerMask;
    void Start()
    {
        InitializeStates();
    }
    private void InitializeStates(){
        CameraLooking looking = new CameraLooking(gameObject, baseTurnrate, lookPoints, lookDuration, cameraHead);
        CameraSeeingPlayer seeingPlayer = new CameraSeeingPlayer(gameObject, cameraHead, seenPlayerTurnRate);

        AddNode(looking, true);
        AddNode(seeingPlayer);

        AddTransition(looking, seeingPlayer, new Predicate(() => CanSeePlayer()));
    }
    public Transform[] GetLookPoints() => lookPoints;
    public void RotateToPoint(Vector3 point){
        Vector3 diff = point - cameraHead.position;
        //Vector3 newDirection = Vector3.RotateTowards(cameraHead.forward, diff, turnRate * Time.deltaTime, 0.0f);
        cameraHead.rotation = Quaternion.LookRotation(diff.normalized);
    }

    private bool CanSeePlayer() => coneDetector.PlayerInSpotlight(GameManager.GetPlayerTransform()) && PlayerNotObstructed();
    private bool PlayerNotObstructed() => !Physics.Linecast(cameraHead.position, GameManager.GetPlayerTransform().position, playerMask);
}
