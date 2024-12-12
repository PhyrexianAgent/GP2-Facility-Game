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

    public void RotateCamera(Quaternion rotation){ // Will handle rotating camera head and base properly (called from states)
        
        //cameraBase.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        //cameraHead.eulerAngles = new Vector3(rotation.eulerAngles.x, 0, rotation.eulerAngles.z);
        cameraHead.rotation = rotation;
    }
}
