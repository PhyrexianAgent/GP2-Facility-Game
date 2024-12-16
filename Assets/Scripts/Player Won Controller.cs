using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerWonController : MonoBehaviour
{
    [SerializeField] private Button retryButton, quitButton;
    [SerializeField] private TMP_Text workbotsCollectedText;
    private Animator anim;
    void Awake(){
        GameManager.PlayerWonGUI = this;
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    public void PlayerWon(){
        workbotsCollectedText.text = $"{GameManager.WorkbotsCollectedCount} Work Bots Have Been Collected";
        anim.enabled = true;
    }
    public void QuitPressed(){
        GameManager.ChangeScene("MainMenu");
    }
    public void RetryPressed(){
        GameManager.ChangeScene("Level 1");
    }
}
