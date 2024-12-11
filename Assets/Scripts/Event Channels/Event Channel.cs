using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChannel<T> : ScriptableObject
{
    private HashSet<EventListener<T>> listeners = new HashSet<EventListener<T>>();
    public void AddListener(EventListener<T> listener) => listeners.Add(listener);
    public void RemoveListener(EventListener<T> listener) => listeners.Remove(listener);

    public void Invoke(T value){
        foreach (EventListener<T> listener in listeners) listener.Raise(value);
    }
}