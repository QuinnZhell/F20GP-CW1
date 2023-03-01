using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] PlayerMovement player;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
            player.StartSwimming();
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
            player.StartWalking();
    }
}
