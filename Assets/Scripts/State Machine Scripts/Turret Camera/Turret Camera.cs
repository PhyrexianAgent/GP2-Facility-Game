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

        AddNode(looking, true);
    }
}
