using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameDialogController : MonoBehaviour
{
    [SerializeField] private DialogTextController textController;
    [SerializeField] private Image characterIconBack;
    [SerializeField] private Image characterIcon;
    [SerializeField] private CanvasGroup dialogGroup;

    private DialogManager currentManager;
    private bool finishedCurrentDialog = false;
    private Image dialogBack;
    private List<Func<bool>> endCallMethods = new List<Func<bool>>();
    void Awake(){
        dialogBack = GetComponent<Image>();
        GameManager.CurrentDialogGUI = this;
        GameManager.PlayerInterface = transform.parent;
    }

    void Start(){
        SetVisible(false);
    }

    void Update(){
        if (!finishedCurrentDialog && Input.GetKeyUp(KeyCode.Space) && textController.TextDone){
            UpdateDialog();
        }
    }
    private void UpdateDialog(){
        //Debug.Log("next dialog");
        finishedCurrentDialog = currentManager.SetNextDialog();
        if (finishedCurrentDialog)
            EndDialog();
        else
            SetCurrentDialog();
    }
    private void EndDialog(){
        SetVisible(false);
        for (int i=0; i<endCallMethods.Count; i++){
            endCallMethods[0]();
            endCallMethods.RemoveAt(0);
        }
    }
    public void StartDialog(DialogController controller){
        finishedCurrentDialog = false;
        currentManager = controller.GetCurrentManager();
        if (currentManager != null){
            SetVisible(true);
            SetCurrentDialog();
        }
    }
    private void SetCurrentDialog(){
        if (currentManager.CurrentDialogTriggersKill()){
            SetVisible(false);
            Debug.Log("begin kill animation (not used for now)");
            //GameManager.StartKillAnimation();
        } 
        else{
            if (!dialogBack.enabled) SetVisible(true);
            textController.SetWholeDialog(currentManager.GetCurrentText());
            characterIcon.sprite = currentManager.GetCurrentSprite();
            SetPortraitVisible(currentManager.GetCurrentSprite() != null);
        }
        
    }
    private void SetVisible(bool visible){
        dialogGroup.alpha = visible ? 1 : 0;
        //dialogBack.enabled = visible;
        //textController.SetTextVisibility(visible);
        GameManager.PauseInput = visible;
        //Debug.Log(visible);
        //SetPortraitVisible(visible);
    }
    private void SetPortraitVisible(bool visible){
        characterIcon.enabled = visible;
        characterIconBack.enabled = visible;
    }
    public void AddEndDialogMethod(Func<bool> method) => endCallMethods.Add(method);
    public void FinishedKillAnimation() => UpdateDialog();
}
