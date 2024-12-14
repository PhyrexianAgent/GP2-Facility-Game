using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigBotStateMachine : StateMachine // Make detection area bigger when it is looking for player
{
    public static BigBotStateMachine instance;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float searchingCoverSpeed;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private AwareConeDetector coneDetector;
    //[SerializeField] private Transform lookPoint;
    [SerializeField] private Transform groundPoint;
    [SerializeField, Min(0)] private float peerOverCoverDistance; // Distance away from cover bot will stop at when running to cover
    [SerializeField] private bool canAttackPlayer = true;

    private NavMeshAgent navAgent;
    //private IState tempStateKey;
    
    void Awake(){
        instance = this;

    }

    void Start(){
        MakeAndSetStates();
    }

    void OnDrawGizmos(){
        Debug.DrawLine(groundPoint.position, groundPoint.position + groundPoint.forward * peerOverCoverDistance, Color.blue);
    }

    void MakeAndSetStates(){
        Patrolling patrolling = new Patrolling(gameObject, patrolPoints, patrolSpeed, canAttackPlayer);
        LookingBehindCover searching = new LookingBehindCover(gameObject, 5f, searchingCoverSpeed);
        ShootingPlayer shooting = new ShootingPlayer(gameObject);
        HolsteringGun holstering = new HolsteringGun(gameObject);
        EquippingGun equipping = new EquippingGun(gameObject, () => CanSeePlayer(true));

        AddNode(patrolling, true);
        AddNode(searching);
        AddNode(shooting);
        AddNode(holstering);
        AddNode(equipping);

        AddTransition(patrolling, equipping, new Predicate(() => CanSeePlayer(false) && patrolling.CanAttack));
        AddTransition(searching, holstering, new Predicate(() => searching.DoneLooking()));
        AddTransition(searching, shooting, new Predicate(() => CanSeePlayer(true)));
        AddTransition(equipping, searching, new Predicate(() => equipping.FinishedAnimation() && !equipping.CanSeePlayer()), PresetSearchDestination);
        AddTransition(equipping, shooting, new Predicate(() => equipping.CanSeePlayer() && equipping.FinishedAnimation()));
        AddTransition(shooting, patrolling, new Predicate(() => shooting.FinishedAnimation()), ForcePatrolAttackCooldown);
        AddTransition(holstering, patrolling, new Predicate(() => holstering.FinishedAnimation()));
        AddTransition(holstering, equipping, new Predicate(() => CanSeePlayer(true)));
    }

    bool CanSeePlayer(bool useAwareDetector){
        return (useAwareDetector ? coneDetector.PlayerInAwareSpotlight(GameManager.GetPlayerTransform()) : coneDetector.PlayerInSpotlight(GameManager.GetPlayerTransform())) && GameManager.PlayerInView(coneDetector.transform.position);// && playerPane.PaneVisibleToPoint(coneDetector.transform.position);
    } 

    bool PresetSearchDestination(IState to){ // Method to set looking destination before state's OnEnter method runs
        StateNode node = GetNode(to);
        LookingBehindCover state = (LookingBehindCover)node.State;
        state.SetLookingDestination(GetObstacleLookingDestination());
        return true;
    }
    bool ForcePatrolAttackCooldown(IState to){ // Temp method while player will not die, will force a 5 second period of patrolling without attacking
        StateNode node = GetNode(to);
        Patrolling state = (Patrolling)node.State;
        state.SaveAttackCooldown(StartCoroutine(state.AttackCooldown()));
        return true;
    }

    Vector3 GetObstacleLookingDestination(){ // Will shoot a ray from bots feet to obstacle, so a position a certain distance from obstacle can be returned
        Transform obstacleTrans = GetObstacleTransform();
        Vector3 dirFromBot = obstacleTrans.position - groundPoint.position;
        RaycastHit hit;
        dirFromBot.Normalize();
        Physics.Raycast(groundPoint.position, dirFromBot, out hit, 500);
        return hit.point - dirFromBot * peerOverCoverDistance;
    }

    Transform GetObstacleTransform(){ // Shoots a raycast from player towards bot, noting the first transform hit, which should be an obstacle
        RaycastHit hit;
        Vector3 dirFromPaneToBot = groundPoint.position - GameManager.GetPlayerTransform().position;
        dirFromPaneToBot.Normalize();
        if (!Physics.Raycast(GameManager.GetPlayerTransform().position, dirFromPaneToBot, out hit, 500)){
            return null;
        }
        Debug.Log(hit.collider.name);
        return hit.collider.transform;
    }
}
