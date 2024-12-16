using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeadController : MonoBehaviour
{
    [SerializeField] private Button quitButton, retryButton;
    private Animator anim;
    void Awake(){
        GameManager.PlayerDeadGUI = this;
        anim = GetComponent<Animator>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerDied(){
        anim.enabled = true;
    }

    public void QuitPressed(){
        GameManager.ChangeScene("MainMenu");
    }
    public void RetryPressed(){
        GameManager.RestartScene();
    }
}
