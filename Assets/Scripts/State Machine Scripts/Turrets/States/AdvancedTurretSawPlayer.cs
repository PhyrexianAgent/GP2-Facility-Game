using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AdvancedTurretSawPlayer : State
{
    public bool DoneLooking {get; private set;}
    private GameObject turretHead;
    private float lookDuration;
    private Coroutine lookWait;
    private AdvancedTurret script;
    private Transform lookPoint;
    private AudioSource _audio;
    private Animator anim;
    public AdvancedTurretSawPlayer(GameObject agent, GameObject turretHead, float lookDuration, Transform lookPoint, AudioSource audio) : base(agent) {
        this.turretHead = turretHead;
        this.lookDuration = lookDuration;
        this.lookPoint = lookPoint;
        script = agent.GetComponent<AdvancedTurret>();
        anim = agent.GetComponent<Animator>();
        _audio = audio;
    }
    public override void OnEnter()
    {
        if (anim != null){
            //Debug.Log("spinning up");
            anim.SetTrigger("Spin Up");
        } 

        if (!_audio.isPlaying)
        {
            _audio.time = _audio.clip.length * 0.15f;
            _audio.Play();
        }
    }
    public override void Update(){
        if (lookWait == null && !DoneLooking){
            TurnToPlayer();
            if (!GameManager.PlayerInView(lookPoint.position)) lookWait = script.StartCoroutine(LookWait());
        }
        else if (!DoneLooking && GameManager.PlayerInView(lookPoint.position)) script.StopCoroutine(lookWait);
    }
    public override void OnExit(){
        if (_audio.isPlaying)
        {
            _audio.Stop();
        }

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
