using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestController : MonoBehaviour
{
    [SerializeField] private SoundChannel soundChannel;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T)){
            soundChannel.Invoke(new Sound(transform.position, 100, 100));
        }
    }
}
