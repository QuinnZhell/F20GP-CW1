using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] Boss boss;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            boss.Activate();
            Destroy(gameObject);
        }
    }
}
