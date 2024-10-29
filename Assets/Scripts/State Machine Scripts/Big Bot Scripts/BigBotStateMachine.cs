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
    [SerializeField] private Transform lookPoint;
    [SerializeField] private Transform groundPoint;
    [SerializeField, Min(0)] private float lookRadius, lookDistance, awareLookRadius, awareLookDistance;
    [SerializeField] private ViewPane playerPane;
    [SerializeField, Min(0)] private float peerOverCoverDistance; // Distance away from cover bot will stop at when running to cover
    [SerializeField] private bool canAttackPlayer = true;

    private NavMeshAgent navAgent;
    private Transform player;
    //private IState tempStateKey;
    
    void Awake(){
        player = playerPane.transform.parent;
        instance = this;

    }

    void Start(){
        MakeAndSetStates();
    }

    void OnDrawGizmos(){
        DrawSpotlight(lookPoint, lookRadius, lookDistance, Color.blue);
        DrawSpotlight(lookPoint, awareLookRadius, awareLookDistance, Color.red);
        Debug.DrawLine(groundPoint.position, groundPoint.position + groundPoint.forward * peerOverCoverDistance, Color.blue);
    }

    void MakeAndSetStates(){
        Patrolling patrolling = new Patrolling(gameObject, patrolPoints, patrolSpeed, canAttackPlayer);
        LookingBehindCover searching = new LookingBehindCover(gameObject, 5f, searchingCoverSpeed);
        ShootingPlayer shooting = new ShootingPlayer(gameObject);
        HolsteringGun holstering = new HolsteringGun(gameObject);
        EquippingGun equipping = new EquippingGun(gameObject, playerPane.transform, () => CanSeePlayer(awareLookDistance, awareLookRadius));

        AddNode(patrolling, true);
        AddNode(searching);
        AddNode(shooting);
        AddNode(holstering);
        AddNode(equipping);

        AddTransition(patrolling, equipping, new Predicate(() => CanSeePlayer(lookDistance, lookRadius) && patrolling.CanAttack));
        AddTransition(searching, holstering, new Predicate(() => searching.DoneLooking()));
        AddTransition(searching, shooting, new Predicate(() => CanSeePlayer(awareLookDistance, awareLookRadius)));
        AddTransition(equipping, searching, new Predicate(() => equipping.FinishedAnimation() && !equipping.CanSeePlayer()), PresetSearchDestination);
        AddTransition(equipping, shooting, new Predicate(() => equipping.CanSeePlayer() && equipping.FinishedAnimation()));
        AddTransition(shooting, patrolling, new Predicate(() => shooting.FinishedAnimation()), ForcePatrolAttackCooldown);
        AddTransition(holstering, patrolling, new Predicate(() => holstering.FinishedAnimation()));
        AddTransition(holstering, equipping, new Predicate(() => CanSeePlayer(awareLookDistance, awareLookRadius)));
    }

    void DrawSpotlight(Transform point, float radius, float length, Color color){
        Vector3 forward = point.position + point.forward * length;
        Vector3 left = forward - point.right * radius;
        Vector3 right = forward + point.right * radius;
        Vector3 up = forward + point.up * radius;
        Vector3 down = forward - point.up * radius;

        Debug.DrawLine(point.position, left, color, 0);
        Debug.DrawLine(point.position, right, color, 0);
        Debug.DrawLine(point.position, up, color, 0);
        Debug.DrawLine(point.position, down, color, 0);

        Debug.DrawLine(left, up, color, 0);
        Debug.DrawLine(up, right, color, 0);
        Debug.DrawLine(right, down, color, 0);
        Debug.DrawLine(down, left, color, 0);
    }

    bool CanSeePlayer(float lookDistance, float lookRadius){
        return PlayerInSight(lookDistance, lookRadius) && playerPane.PaneVisibleToPoint(lookPoint.position);
    } 

    bool PlayerInSight(float lookDistance, float lookRadius){ // Similar to what we did in class but with a cone so such a cone can be made easily visible
        Vector3 forward = lookPoint.position + lookPoint.forward * lookDistance;
        float coneDist = Vector3.Dot(playerPane.transform.position - lookPoint.position, lookPoint.forward);

        if (coneDist < 0 || coneDist > lookDistance)
            return false;

        Vector3 coneDistPoint = lookPoint.position + lookPoint.forward * coneDist;
        float coneRadius = (coneDist / lookDistance) * lookRadius;

        return Vector3.Distance(coneDistPoint, playerPane.transform.position) <= coneRadius;
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
        Vector3 dirFromPaneToBot = groundPoint.position - playerPane.transform.position;
        dirFromPaneToBot.Normalize();
        if (!Physics.Raycast(playerPane.transform.position, dirFromPaneToBot, out hit, 500)){
            return null;
        }
        Debug.Log(hit.collider.name);
        return hit.collider.transform;
    }
}
