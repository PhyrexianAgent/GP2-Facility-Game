using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbotController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundHeard(Sound sound){
        Debug.Log(sound.CanHearSound(transform.position));
    }
}
