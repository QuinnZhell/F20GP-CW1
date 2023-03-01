using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    [SerializeField] AudioManager audioManager;

    private void Awake() {
        audioManager = Object.FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            player.StartSwimming();
            audioManager.playAllSounds();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            player.StartWalking();
            audioManager.stopAllSounds();
        }
    }
}
