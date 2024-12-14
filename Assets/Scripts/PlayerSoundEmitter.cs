using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEmitter : MonoBehaviour
{
    [SerializeField] private SoundChannel soundChannel;

    [Header("Walking")]
    [SerializeField] private AudioClip[] walkSounds;
    [SerializeField, Min(0)] private float walkSoundSize;
    


    private AudioSource audio;
    void Awake(){
        audio = GetComponent<AudioSource>();
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * walkSoundSize);
    }
    public void PlayWalkStep(){
        audio.Stop();
        audio.clip = walkSounds[Random.Range(0, walkSounds.Length)];
        audio.Play();
        //soundChannel.Invoke(new Sound(transform.position, walkSoundSize, 10));
    }
    public void PlayNoSound() => audio.Stop();
}
