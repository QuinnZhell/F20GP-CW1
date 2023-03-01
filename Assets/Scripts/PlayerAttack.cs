using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    bool attacking = false;

    // Update is called once per frame
    void Update()
    {
        if(!attacking) {
            if(Input.GetButtonDown("Attack"))
                Attack();
        }
    }

    IEnumerator Attack(){
        attacking = true;
        Debug.Log("click");
        yield return new WaitForSeconds(2);
        attacking = false;
    }
}
