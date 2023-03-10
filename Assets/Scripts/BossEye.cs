using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEye : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] GameObject projectile;

    private void Awake() {
        boss = FindAnyObjectByType<Boss>();
    }

    // something hit eye, destroy it
    public void Shot() {
        boss.DestroyEye(this);
        Destroy(gameObject);
    }

    // create a projectile that will head towards player
    public void Fire() {
        Instantiate(projectile, transform.position + transform.InverseTransformDirection(new Vector3(0,0,-2)), Quaternion.identity);
    }
}
