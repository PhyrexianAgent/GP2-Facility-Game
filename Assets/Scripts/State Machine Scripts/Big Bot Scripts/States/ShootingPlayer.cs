using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlayer : State
{
    private const string SHOOT_ANIM = "Shoot 2";
    public bool FinishedAnimation() => animator.GetCurrentAnimatorStateInfo(0).IsName(SHOOT_ANIM) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0);
    public ShootingPlayer(GameObject agent) : base(agent){

    }

    public override void OnEnter(){
        animator.Play(SHOOT_ANIM, 0);
        Debug.Log("Shot Player");
    }
}
