using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialog/Dialog Manager")]
public class DialogManager : ScriptableObject
{
    [SerializeField] private Dialog[] dialogs;

    private int currentDialogIndex = 0;
    public string GetCurrentText() => dialogs[currentDialogIndex].Text;
    public bool SetNextDialog(){
        currentDialogIndex ++;
        return currentDialogIndex >= dialogs.Length;
    } 
    public Sprite GetCurrentSprite() => GameManager.GetCharacterHead(dialogs[currentDialogIndex].Source);
    public void Reset() => currentDialogIndex = 0;
    public bool CurrentDialogTriggersKill(){
        if (dialogs.Length > 0 && currentDialogIndex < dialogs.Length) return dialogs[currentDialogIndex].TriggerKillAnimation;
        return false;
    }// => if (dialogs.Length > 0){ dialogs[currentDialogIndex].TriggerKillAnimation;
}
