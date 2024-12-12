using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCamera : StateMachine
{
    [SerializeField] Transform cameraHead, cameraBase;
    [SerializeField] Transform[] lookPoints; // Camera will look between a bunch of points in this list (makes designing where camera looks easier)
    [SerializeField, Min(0)] float baseTurnrate, seenPlayerTurnRate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateCamera(Quaternion rotation){ // Will handle rotating camera head and base properly (called from states)

    }
}
