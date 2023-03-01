using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{

    public GameObject audioManager;
    private AudioSource[] sources;

    // Set the audio sources as soon as possible
    void Awake()
    {
        sources = audioManager.GetComponents<AudioSource>();
    }

    // silence all sounds
    public void stopAllSounds() {
        foreach(AudioSource source in sources){
            source.Pause();
        }
    }

    // play all sounds
    public void playAllSounds() {
        foreach(AudioSource source in sources){
            source.Play();
        }
    }

}
