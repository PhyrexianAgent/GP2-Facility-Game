using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavKeypad;
using System;

public class KeypadController : MonoBehaviour
{
    private static int activatedKeypadCount = 0;
    [SerializeField, Min(0)] private float keyPressDelay = 0.3f;
    [SerializeField] private int keypadsNeededToContinue = 1;
    [SerializeField] private GameObject doorObject;
    [SerializeField] private AudioSource doorAudioController;
    private Keypad keypadScript; 
    void Awake()
    {
        activatedKeypadCount = 0;
        keypadScript = GetComponent<Keypad>();
    }

    public void EnterCombo(){
        StartCoroutine(EnterKeys());
    }

    private IEnumerator EnterKeys(){
        string code = keypadScript.GetCombo();
        GameManager.PauseInput = true;
        for (int i=0; i<code.Length; i++){
            keypadScript.AddInput($"{code[i]}");
            yield return new WaitForSeconds(keyPressDelay);
        }
        keypadScript.AddInput("enter");
        GameManager.PauseInput = false;
        activatedKeypadCount++;
        if (activatedKeypadCount == keypadsNeededToContinue){
            GameManager.CurrentElevatorEntrance.Unlock();
            doorObject.SetActive(false);
            doorAudioController.Play();
        } 
    }
}
