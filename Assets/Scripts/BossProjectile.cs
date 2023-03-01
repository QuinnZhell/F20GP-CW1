using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    bool deflected = false;
    Vector3 target;
    PlayerCamera player;
    float timer = 0f;
    GameManager gm;

    private void Awake() {
        player = FindAnyObjectByType<PlayerCamera>();
        gm = FindAnyObjectByType<GameManager>();
        target = player.transform.position;
    }

    private void Update() {
        timer += Time.deltaTime;

        if(!deflected) {
            UpdateTarget();
            transform.position = Vector3.MoveTowards(transform.position, target, 0.15f);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + target, 0.15f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("collision");

        switch (other.tag) {
            case "Player":
                Debug.Log("Player took damage");
                gm.applyDamage(10.0f);
                Destroy(gameObject);
                break;
            case "PlayerAttack":
                Debug.Log("Switching Owner");
                Deflect();
                break;
            case "Enemy":
                other.GetComponent<BossEye>().Shot();
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    private void Deflect() {
        deflected = true;
        target = transform.InverseTransformDirection(player.transform.forward);
    }

    void UpdateTarget() {
        if(timer > 1f){
            //target = player.transform.position;
            timer = 0;
        }
    }
}
