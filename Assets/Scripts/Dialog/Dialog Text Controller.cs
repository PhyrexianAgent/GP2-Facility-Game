using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogTextController : MonoBehaviour
{
    public bool TextDone {get; private set;} = false;
    private string wholeDialog;
    [SerializeField, Range(0, 1)] private float addTextSpeed = 0.3f;
    private TMP_Text tmp;
    private Coroutine addText;
    private bool textWasAdded = false;

    void Awake(){
        tmp = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !TextDone && !textWasAdded){
            SetRemainingText();
        }
    }

    IEnumerator AddTextOverTime(){
        yield return null;
        for (int i=1; i<wholeDialog.Length; i++){
            tmp.text = wholeDialog.Substring(0, i);
            yield return new WaitForSeconds(addTextSpeed/5);
        }
        TextDone = true;
    }

    public void SetWholeDialog(string dialog){
        if (addText != null) StopCoroutine(addText);
        StartCoroutine(DelayTextAdded());
        TextDone = false;
        wholeDialog = dialog;
        tmp.text = "";
        if (addTextSpeed > 0){
            addText = StartCoroutine(AddTextOverTime());
        }
        else{
            tmp.text = wholeDialog;
            TextDone = true;
        }
        
    }
    public void SetTextVisibility(bool visible) => tmp.enabled = visible;
    private void SetRemainingText(){
        TextDone = true;
        if (addText != null) StopCoroutine(addText);
        tmp.text = wholeDialog;
    }
    private IEnumerator DelayTextAdded(){
        textWasAdded = true;
        yield return null;
        textWasAdded = false;
    }
}
