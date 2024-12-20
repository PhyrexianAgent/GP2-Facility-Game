using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCamera : StateMachine
{
    [SerializeField] Transform cameraHead, cameraBase;
    [SerializeField] Transform[] lookPoints; // Camera will look between a bunch of points in this list (makes designing where camera looks easier)
    [SerializeField] BasicTurret[] basicTurrets;
    [SerializeField] AdvancedTurret[] advancedTurrets;
    [SerializeField, Min(0)] float baseTurnrate, seenPlayerTurnRate, lookDuration, lostPlayerDuration;
    [SerializeField] private ConeDetector coneDetector;
    [SerializeField] private LayerMask detectIgnoreMask;
    void Start()
    {
        InitializeStates();
        foreach (AdvancedTurret turret in advancedTurrets) turret.SetControllingCamera(gameObject);
    }
    private void InitializeStates(){
        CameraLooking looking = new CameraLooking(gameObject, baseTurnrate, lookPoints, lookDuration, cameraHead, basicTurrets, advancedTurrets);
        CameraSeeingPlayer seeingPlayer = new CameraSeeingPlayer(gameObject, cameraHead, seenPlayerTurnRate, basicTurrets, advancedTurrets);
        CameraLostPlayer lostPlayer = new CameraLostPlayer(gameObject, lostPlayerDuration);

        AddNode(looking, true);
        AddNode(seeingPlayer);
        AddNode(lostPlayer);

        AddTransition(looking, seeingPlayer, new Predicate(() => CanSeePlayer()));
        AddTransition(seeingPlayer, lostPlayer, new Predicate(() => !GameManager.PlayerInView(cameraHead.position)));
        AddTransition(lostPlayer, looking, new Predicate(() => lostPlayer.FinishedDelay));
        AddTransition(lostPlayer, seeingPlayer, new Predicate(() => GameManager.PlayerInView(cameraHead.position)));
    }
    public Transform[] GetLookPoints() => lookPoints;
    public void RotateToPoint(Vector3 point){
        Vector3 diff = point - cameraHead.position;
        cameraHead.rotation = Quaternion.LookRotation(diff.normalized);
    }

    private bool CanSeePlayer() => coneDetector.PlayerInSpotlight(GameManager.GetPlayerTransform()) && GameManager.PlayerInView(cameraHead.position);//PlayerNotObstructed();
    private bool PlayerNotObstructed() {
        RaycastHit hit;
        bool hitSomething = Physics.Linecast(cameraHead.position, GameManager.GetPlayerTransform().position, out hit);
        //Debug.DrawLine(cameraHead.position, hit.point, Color.red, 0);
        return hitSomething && hit.collider.tag == "Player";
    }
}
