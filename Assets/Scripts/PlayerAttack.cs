using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] bool attacking = false;
    [SerializeField] GameObject attackVisual;
    PlayerCamera player;

    private void Awake() {
        player = FindAnyObjectByType<PlayerCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        // only able to attack if an attack isn't in progress
        if(!attacking) {
            if(Input.GetButtonDown("Attack"))
                StartCoroutine(Attack()); 
        }
    }

    IEnumerator Attack(){
        attacking = true;

        StartCoroutine(AttackAnimate());
        
        RaycastHit hit;
        if(Physics.SphereCast(player.transform.position, 1, player.transform.forward, out hit, 1.25f)) {
            switch(hit.collider.tag) {
                case "Shark":
                    Debug.Log("Shark Smacked");
                    hit.collider.GetComponent<sharkBehaviour>().Hurt();
                    break;
                default:
                    break;
            }
        }

        // player is unable to 'spam' the attack button
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    // Displays a visual indicator of the players attack radius
    IEnumerator AttackAnimate() {
        // create the indicator then remove after 0.25 seconds
        GameObject atk = Instantiate(attackVisual, player.transform.position + player.transform.forward * 1, Quaternion.identity, player.transform);
        yield return new WaitForSeconds(.25f);
        Destroy(atk);
    }
}
