using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] public bool locked = true;
    private bool opening = false;

    // inform the game manager of this object
    private void Awake() {
        FindObjectOfType<GameManager>().SetActiveDoor(this);
    }

    // Update is called once per frame
    void Update()
    {
        // if door is unlocked, check for the player interacting
        if(!locked && Input.GetButtonDown("Interact") && interactive){
            Debug.Log("interact");
            Open();
        }

        // slowly slide open
        if(opening)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.InverseTransformDirection(new Vector3(0,0,4)), .01f);
    }

    // allow the door to be unlocked
    public void Unlock() {
        text = "Press E to open door.";
        locked = false;
    }

    // open the door
    void Open() {
        Debug.Log("opening");
        opening = true;
    }
}
