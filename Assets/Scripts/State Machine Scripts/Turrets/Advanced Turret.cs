using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTurret : StateMachine
{
    [SerializeField] private Transform[] lookPoints;
    [SerializeField, Min(0)] private float idleTurnRate, alertedTurnRate, lookDuration;
    [SerializeField] private Transform turretHead;
    void Start()
    {
        InitializeStates();
    }

    private void InitializeStates(){
        CameraLooking idle = new CameraLooking(gameObject, idleTurnRate, lookPoints, lookDuration, turretHead);

        AddNode(idle, true);

        
    }
}
