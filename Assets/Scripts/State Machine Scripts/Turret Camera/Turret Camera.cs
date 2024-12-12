using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCamera : StateMachine
{
    [SerializeField] Transform cameraHead, cameraBase;
    [SerializeField] Transform[] lookPoints; // Camera will look between a bunch of points in this list (makes designing where camera looks easier)
    [SerializeField, Min(0)] float baseTurnrate, seenPlayerTurnRate, lookDuration;
    [SerializeField] private ConeDetector coneDetector;
    void Start()
    {
        InitializeStates();
    }
    private void InitializeStates(){
        CameraLooking looking = new CameraLooking(gameObject, baseTurnrate, lookPoints, lookDuration, cameraHead);
        CameraSeeingPlayer seeingPlayer = new CameraSeeingPlayer(gameObject, cameraHead, seenPlayerTurnRate);

        AddNode(looking, true);
        AddNode(seeingPlayer);

        AddTransition(looking, seeingPlayer, new Predicate(() => coneDetector.PlayerInSpotlight(GameManager.GetPlayerTransform())));
    }
}
