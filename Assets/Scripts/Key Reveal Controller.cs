using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRevealController : MonoBehaviour
{
    [SerializeField] private Transform keyPosition;
    [SerializeField] private Sprite keyIcon;
    [SerializeField] private float triggerDistance;

    private KeyCode keyToPress = KeyCode.E;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * triggerDistance);
    }
}
