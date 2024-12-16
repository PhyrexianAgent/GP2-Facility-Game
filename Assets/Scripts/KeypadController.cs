using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavKeypad;
using System;

public class KeypadController : MonoBehaviour
{
    [SerializeField, Min(0)] private float keyPressDelay = 0.3f;
    private Keypad keypadScript; 
    void Awake()
    {
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
        GameManager.CurrentElevatorEntrance.Unlock();
    }
}
