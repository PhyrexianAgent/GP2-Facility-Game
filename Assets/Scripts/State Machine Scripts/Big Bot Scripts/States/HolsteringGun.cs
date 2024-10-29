using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolsteringGun : State
{
    private const string HOLSTERING_ANIM = "HolsterCannon";
    public bool FinishedAnimation() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0);
    public HolsteringGun(GameObject agent) : base(agent){

    }
    public override void OnEnter(){
        animator.Play(HOLSTERING_ANIM, 0);
    }
}
