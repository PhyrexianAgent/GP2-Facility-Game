using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        GameManager.CurrentSceneFader = this;
    }
    public void FadeIn(){
        anim.Play("Fade In");
    }
}
