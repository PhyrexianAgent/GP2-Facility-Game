using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraSeeingPlayer : State
{
    private Transform cameraHead;
    private float turnRate;
    private BasicTurret[] basicTurrets;
    private AdvancedTurret[] advancedTurrets;

    public CameraSeeingPlayer(GameObject agent, Transform cameraHead, float turnRate, BasicTurret[] basicTurrets, AdvancedTurret[] advancedTurrets) : base(agent){
        this.cameraHead = cameraHead;
        this.turnRate = turnRate;
        this.basicTurrets = basicTurrets;
        this.advancedTurrets = advancedTurrets;
    }
    public override void OnEnter(){
        foreach(BasicTurret turret in basicTurrets) 
        {
            turret.IsActive = true;

            GameObject effect = FindDescendant(turret.transform, "Shooting_ParticleSystem");
            AudioSource audioSource = turret.GetComponent<AudioSource>();
            effect.SetActive(true);

            if (!audioSource.isPlaying)
            {
                audioSource.time = audioSource.clip.length * 0.15f;
                audioSource.Play();
            }
        }
        foreach (AdvancedTurret turret in advancedTurrets)
        {
            turret.ActiveFromCamera = true;
        }
    }
    public override void Update(){
        RotateToPlayer();
        SetTurretsTarget();
    }
    private void RotateToPlayer(){
        Vector3 diff = GameManager.GetPlayerTransform().position - cameraHead.position;
        Vector3 newDirection = Vector3.RotateTowards(cameraHead.forward, diff, turnRate * Time.deltaTime, 0.0f);
        cameraHead.rotation = Quaternion.LookRotation(newDirection);
    }
    private void SetTurretsTarget(){
        Vector3 playerPos = GameManager.GetPlayerTransform().position;
        foreach(BasicTurret turret in basicTurrets) turret.CurrentTarget = playerPos;
        foreach(AdvancedTurret turret in advancedTurrets) turret.CurrentTarget = playerPos;
    }

    public override void OnExit()
    {
        foreach (BasicTurret turret in basicTurrets)
        {
            turret.IsActive = false;

            GameObject effect = FindDescendant(turret.transform, "Shooting_ParticleSystem");
            AudioSource audioSource = turret.GetComponent<AudioSource>();
            effect.SetActive(false);

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        foreach (AdvancedTurret turret in advancedTurrets)
        {
            turret.ActiveFromCamera = false;
        }
    }

    GameObject FindDescendant(Transform parent, string target)
    {
        // Check if current descendant matches
        if (parent.gameObject.name == target)
        {
            return parent.gameObject;
        }

        // Search through all descendants
        foreach (Transform child in parent)
        {
            GameObject found = FindDescendant(child, target);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
}
