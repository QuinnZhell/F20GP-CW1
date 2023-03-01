using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{

    public GameObject audioManager;
    private AudioSource[] sources;

    // Start is called before the first frame update
    void Awake()
    {
        sources = audioManager.GetComponents<AudioSource>();
    }

    public void stopAllSounds() {
        foreach(AudioSource source in sources){
            source.Pause();
        }
    }

    public void playAllSounds() {
        foreach(AudioSource source in sources){
            source.Play();
        }
    }

}
