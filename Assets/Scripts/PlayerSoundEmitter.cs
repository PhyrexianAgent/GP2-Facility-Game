using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEmitter : MonoBehaviour
{
    [SerializeField] private SoundChannel soundChannel;

    [Header("Walking")]
    [SerializeField] private AudioClip[] walkSounds;
    [SerializeField] private float walkSoundSize;
    [SerializeField, Range(0, 2)] private float walkVolume = 1;

    [Header("Running")]
    [SerializeField] private AudioClip[] runSounds;
    [SerializeField] private float runSoundSize;
    [SerializeField, Range(0, 2)] private float runVolume = 1;

    [Header("Sneaking")]
    [SerializeField] private float sneakSoundSize;
    [SerializeField, Range(0, 2)] private float sneakVolume = 0.6f;


    private AudioSource audio;
    void Awake(){
        audio = GetComponent<AudioSource>();
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * walkSoundSize);
    }
    private void PlaySoundFromArray(AudioClip[] sounds, float volume){
        audio.volume = volume;
        audio.Stop();
        audio.clip = sounds[Random.Range(0, sounds.Length)];
        audio.Play();
    }
    public void PlayWalkStep(){
        Debug.Log("playing walk");
        PlaySoundFromArray(walkSounds, walkVolume);
        //soundChannel.Invoke(new Sound(transform.position, walkSoundSize, 10));
    }
    public void PlayRunStep(){
        Debug.Log("playing run");
        PlaySoundFromArray(runSounds, runVolume);
    }
    public void PlaySneakStep(){
        Debug.Log("playing sneak");
        PlaySoundFromArray(walkSounds, sneakVolume);
    }
    public void PlayNoSound(){}// => audio.Stop();
}
