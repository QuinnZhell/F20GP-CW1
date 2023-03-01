using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    GameManager gm;

    private void Awake() {
        gm = FindObjectOfType<GameManager>();
    }

    // kill player if they touch this
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
            gm.WinOrLose(2);
    }
}
