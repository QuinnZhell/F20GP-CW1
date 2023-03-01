using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    bool active = false;
    int eyesRemaining = 3;
    [SerializeField] List<BossEye> eyes;
    [SerializeField] GameObject treasure;
    [SerializeField] GameManager gm;

    enum FirePattern {fire1, fire2, fire3};
    FirePattern firing = FirePattern.fire1;
    float timer = 0f;
    int activeEye = 0;
    

    private void Update() {
        if(active){
            timer += Time.deltaTime;
        
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

    public void Activate() {
        active = true;
    }

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
