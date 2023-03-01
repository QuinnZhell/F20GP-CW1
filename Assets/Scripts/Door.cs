using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] public bool locked = true;
    private bool opening = false;

    private void Awake() {
        FindObjectOfType<GameManager>().SetActiveDoor(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(!locked && Input.GetButtonDown("Interact") && interactive){
            Debug.Log("interact");
            Open();
        }

        if(opening)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.InverseTransformDirection(new Vector3(0,0,4)), .01f);
    }

    public void Unlock() {
        text = "Press E to open door.";
        locked = false;
    }

    void Open() {
        Debug.Log("opening");
        opening = true;
    }
}
