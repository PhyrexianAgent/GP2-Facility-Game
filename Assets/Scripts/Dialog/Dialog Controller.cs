using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialog/Dialog Controller")]
public class DialogController : ScriptableObject
{
    [SerializeField] private DialogManager[] initialDialog, repeatDialog;
    [SerializeField, Min(0)] private int dayIndex = 0;
    [SerializeField] private bool isNightDialog;
    //[SerializeField] private bool randomizeInitialDialog = false;

    private int initialDialogIndex, repeatDialogIndex;
    private bool finishedInitialDialog = false;

    public DialogManager GetCurrentManager(){
        DialogManager manager = null;
        if (!finishedInitialDialog) finishedInitialDialog = initialDialog.Length == 0;
        if (finishedInitialDialog){
            if (repeatDialog.Length > 0) manager = repeatDialog[repeatDialogIndex];
            AddRepeatIndex();
        }
        else{
            if (initialDialog.Length > 0) manager = initialDialog[initialDialogIndex];
            AddInitialIndex();
        }
        return manager;
    }
    public int GetDayIndex() => dayIndex;
    public bool IsNightDialog() => isNightDialog;
    public void Reset(){
        initialDialogIndex = 0;
        repeatDialogIndex = 0;
        finishedInitialDialog = false;
        foreach(DialogManager dialog in initialDialog) dialog.Reset();
        foreach(DialogManager dialog in repeatDialog) dialog.Reset();
    }
    private void ResetRepeatDialog(){
        repeatDialogIndex = 0;
        foreach(DialogManager dialog in repeatDialog) dialog.Reset();
    }

    private void AddRepeatIndex(){
        repeatDialogIndex++;
        if (repeatDialogIndex >= repeatDialog.Length){
            ResetRepeatDialog();
            ShuffleDialogManager(repeatDialog);
        }
    }
    private void AddInitialIndex(){
        initialDialogIndex++;
        finishedInitialDialog = initialDialogIndex >= initialDialog.Length;
    }
    private void ShuffleDialogManager(DialogManager[] managers){
        for (int i=0; i<managers.Length; i++){
            int rand = Random.Range(0, managers.Length);
            DialogManager temp = managers[rand];
            managers[rand] = managers[i];
            managers[i] = temp;
        }
    }
}
