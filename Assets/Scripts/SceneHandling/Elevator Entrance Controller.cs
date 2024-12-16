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

    [Header("Start Dialog")]
    [SerializeField] private DialogManager entranceDialog;
    [SerializeField] private DialogManager lockedDialog;

    private AudioSource audio;

    void Awake(){
        audio = GetComponent<AudioSource>();
        entranceDialog.Reset();
        lockedDialog.Reset();
    }
    void Start(){
        StartCoroutine(PlayStartSounds());
        StartCoroutine(StartLevelDialog());
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

    }

}
