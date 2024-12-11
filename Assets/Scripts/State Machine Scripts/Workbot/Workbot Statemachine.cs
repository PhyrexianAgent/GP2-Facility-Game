using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorkbotStatemachine : StateMachine
{
    private SoundListener soundListener;
    //[SerializeField] private UnityEvent<Sound> soundEvent;
    void Awake(){
        soundListener = GetComponent<SoundListener>();
    }
    void Start()
    {
        Working working = new Working(gameObject, soundListener.GetUnityEvent());

        AddNode(working, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void HeardSound(Sound sound)
}
