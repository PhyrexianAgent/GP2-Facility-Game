using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyRevealController : MonoBehaviour
{ 
    [SerializeField] private Transform keyPosition;
    [SerializeField] private Sprite keyIcon;
    [SerializeField] private float triggerDistance;
    [SerializeField] private GameObject keyIconPrefab;

    private KeyCode keyToPress = KeyCode.E;
    private GameObject currentIcon = null;
    private UnityEvent unityEvent;
    private bool eventCalled = false;
    void Update()
    {
        RevealIconWhenNeeded();
        if (currentIcon != null && Input.GetKeyUp(KeyCode.E)){
            eventCalled = true;
            unityEvent.Invoke();

        }
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * triggerDistance);
    }
    void OnDestroy(){
        unityEvent.RemoveAllListeners();
    }
    private void RemoveIcon(){
        Destroy(currentIcon);
        currentIcon = null;
    }
    private void RevealIconWhenNeeded(){
        float distance = Vector3.Distance(GameManager.GetPlayerTransform().position, transform.position);
        if (distance <= triggerDistance && currentIcon == null && !eventCalled){
            currentIcon = Instantiate(currentIcon, GameManager.PlayerInterface);
            currentIcon.GetComponent<KeyIconController>().GamePoint = keyPosition;
        }
        else if (distance > triggerDistance){
            eventCalled = false;
            if (currentIcon != null) RemoveIcon();
        } 
    }
    public UntiyEvent GetUnityEvent() => unityEvent;
}
