using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class ElevatorEntranceController : MonoBehaviour
{

    [Header("Door Audio")]
    [SerializeField] private AudioClip levelEnd1;
    [SerializeField] private AudioClip levelEnd2;
    [SerializeField] private AudioClip levelBegin1;
    [SerializeField] private AudioClip levelBegin2;

    [Header("Dialog and Scenes")]
    [SerializeField] private DialogManager entranceDialog;
    [SerializeField] private DialogManager lockedDialog;
    [SerializeField] private string nextSceneName;
    [SerializeField] private bool makePlayerWin = false;


    private AudioSource audio;
    private bool locked = true;

    void Awake(){
        audio = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            GameManager.CurrentElevatorEntrance = this;
            entranceDialog.Reset();
            lockedDialog.Reset();
        }
    }
    void Start(){
        StartCoroutine(PlayStartSounds());
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            StartCoroutine(StartLevelDialog());
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private IEnumerator StartLevelDialog(){
        yield return null;
        GameManager.CurrentDialogGUI.StartDialog(entranceDialog);
    }
        /*SFX.PlayOneShot(levelBegin1);
        yield return sceneFade.fadeIn(fadeDuration);
        yield return new WaitForSeconds(0.1f);
        SFX.PlayOneShot(levelBegin2);*/

        /*SFX.PlayOneShot(levelEnd1);
        yield return new WaitForSeconds(0.3f);
        SFX.PlayOneShot(levelEnd2);
        yield return new WaitForSeconds(0.5f);
        yield return sceneFade.fadeOut(fadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);*/
    
        /*SFX.PlayOneShot(levelEnd1);
        yield return new WaitForSeconds(0.3f);
        SFX.PlayOneShot(levelEnd2);
        yield return new WaitForSeconds(0.5f);
        yield return sceneFade.fadeOut(fadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneIndex);*/


    private IEnumerator PlayStartSounds(){
        audio.PlayOneShot(levelBegin1);
        yield return new WaitForSeconds(0.4f);
        audio.PlayOneShot(levelBegin2);
    }
    public void LeaveLevel(){
        if (locked){
            lockedDialog.Reset();
            GameManager.CurrentDialogGUI.StartDialog(lockedDialog);
        }
        else if (makePlayerWin)
            GameManager.PlayerWon();
        else
            GameManager.ChangeScene(nextSceneName);
    }
    public void Unlock() => locked = false;
}
