using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header ("Game Objects")]
    [SerializeField] List<BossEye> eyes;
    [SerializeField] GameObject treasure;
    [SerializeField] GameManager gm;

    // Don't want boss active on init
    bool active = false;
    // once it has no eyes it dies
    int eyesRemaining = 3;

    // Boss Follows three firing patterns dependant on remaining eyes
    enum FirePattern {fire1, fire2, fire3};
    // current firing pattern set to the first on creation
    FirePattern firing = FirePattern.fire1;
    // timer used for spacing attacks
    float timer = 0f;
    // currently firing eye
    int activeEye = 0;
    

    private void Update() {
        if(active){
            timer += Time.deltaTime;
        
            // 3 different firing patterns
            switch (firing) {
                case FirePattern.fire1:
                    FirePattern1();
                    break;
                case FirePattern.fire2:
                    FirePattern2();
                    break;
                case FirePattern.fire3:
                    FirePattern3();
                    break;
            }
        }   
    }

    // set the boss active remotely (from trigger)
    public void Activate() {
        active = true;
    }

    // 3 remaining eyes (full health)
    // first pattern fires projectiles towards player once every 3 seconds
    // changes active eye after each shot
    void FirePattern1() {
        if (timer > 3) {
            eyes[activeEye].Fire();

            if(activeEye == (eyesRemaining-1))
                activeEye = 0;
            else
                activeEye++;

            timer = 0;
        }
    }

    // 2 remaining eyes
    // second patter fires three projectiles towards player in succession
    // fires every 2 seconds, alternates eyes
    void FirePattern2() {
        if (timer > 2) {
            eyes[activeEye].Fire();
            eyes[activeEye].Fire();
            eyes[activeEye].Fire();

            if(activeEye == (eyesRemaining-1))
                activeEye = 0;
            else
                activeEye++;
            
            timer = 0;
        }
    }

    // 1 remaining eye
    // fires a quick burst of projectiles consistently in a short time frame
    // only remaining eye fires
    void FirePattern3() {
        if (timer > 0.5f) {
            eyes[0].Fire();
        }
    }

    // an eye has been hit, destroy it
    public void DestroyEye(BossEye eye) {
        eyesRemaining--;
        activeEye = 0;

        if(eyesRemaining <= 0)
            Die();

        firing = firing + 1;
        eyes.Remove(eye);
    }

    // all eyes dead
    void Die() {

        // make sure no lingering projectiles
        BossProjectile[] remaining = FindObjectsOfType<BossProjectile>();
        foreach(BossProjectile projectile in remaining) {
            Destroy(projectile.gameObject);
        }

        gm.WinOrLose(3);
        Destroy(gameObject);
    }
}
