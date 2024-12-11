using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener<T> : MonoBehaviour
{
    [SerializeField] private EventChannel<T> channel;
    [SerializeField] private UnityEvent<T> unityEvent;
    void Awake()
    {
        channel.AddListener(this);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        channel.RemoveListener(this);
    }

    public void Raise(T value){
        unityEvent?.Invoke(value);
    }
    public UnityEvent<T> GetUnityEvent() => unityEvent;
    public EventChannel<T> GetEventChannel() => channel;
}
