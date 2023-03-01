using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    // projectiles can be deflected back at the boss
    bool deflected = false;
    Vector3 target;
    PlayerCamera player;
    float timer = 0f;
    // game manager relays when a player is hit
    GameManager gm;

    // set the player as the target and the game manager
    private void Awake() {
        player = FindAnyObjectByType<PlayerCamera>();
        gm = FindAnyObjectByType<GameManager>();
        target = player.transform.position;
    }

    // move 0.15f units towards the target each update frame
    private void Update() {
        timer += Time.deltaTime;

        if(!deflected) {
            // if not deflected move towards where the player was on init
            transform.position = Vector3.MoveTowards(transform.position, target, 0.15f);
        } else {
            // if deflected it travels straight
            transform.position = Vector3.MoveTowards(transform.position, transform.position + target, 0.15f);
        }
    }

    // the projectile has hit something
    private void OnTriggerEnter(Collider other) {
        Debug.Log("collision");

        switch (other.tag) {
            case "Player":
                // on collision with player, take damage and destroy projectile
                Debug.Log("Player took damage");
                gm.applyDamage(10.0f);
                Destroy(gameObject);
                break;
            case "PlayerAttack":
                // on collision with playerAttack, change the target and direction
                Debug.Log("Switching Owner");
                Deflect();
                break;
            case "Enemy":
                // on collision with the boss, destroy its eye
                other.GetComponent<BossEye>().Shot();
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    // takes the forward direction of the players camera (where they are looking)
    // tells the projectile to move continously in that direction
    private void Deflect() {
        deflected = true;
        target = transform.InverseTransformDirection(player.transform.forward);
    }
}
