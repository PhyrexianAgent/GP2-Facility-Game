using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTurretSawPlayer : State
{
    public bool DoneLooking {get; private set;}
    private GameObject turretHead;
    private float lookDuration;
    private Coroutine lookWait;
    private MonoBehaviour script;
    private Transform lookPoint;
    public AdvancedTurretSawPlayer(GameObject agent, GameObject turretHead, float lookDuration, Transform lookPoint) : base(agent) {
        this.turretHead = turretHead;
        this.lookDuration = lookDuration;
        this.lookPoint = lookPoint;
        script = agent.GetComponent<MonoBehaviour>();
    }
    public override void Update(){
        if (lookWait == null && !DoneLooking){
            TurnToPlayer();
            if (!GameManager.PlayerInView(lookPoint.position, lookPoint.forward)) lookWait = script.StartCoroutine(LookWait());
        }
        else if (!DoneLooking && GameManager.PlayerInView(lookPoint.position, lookPoint.forward)) script.StopCoroutine(lookWait);
    }
    public override void OnExit(){
        DoneLooking = false;
        if (lookWait != null) script.StopCoroutine(lookWait);
    }

    private void TurnToPlayer(){
        Vector3 dirToPlayer = (GameManager.GetPlayerTransform().position - turretHead.transform.position).normalized;
        turretHead.transform.rotation = Quaternion.LookRotation(dirToPlayer);
    }
    private IEnumerator LookWait(){
        yield return new WaitForSeconds(lookDuration);
        DoneLooking = true;
    }
}
