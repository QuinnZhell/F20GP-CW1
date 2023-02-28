using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool locked = false;
    private bool opening = false;

    // Update is called once per frame
    void Update()
    {
        if(!locked && Input.GetButtonDown("Interact")){
            Debug.Log("interact");
            Open();
        }

        if(opening)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.InverseTransformDirection(new Vector3(0,0,4)), .01f);
    }

    void Open() {
        Debug.Log("opening");
        opening = true;
    }
}
