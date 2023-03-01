using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("External Assignments")]
    Transform body;
    public new Camera camera;
    CharacterController controller;

    [Header ("Movement Stats")]
    public float moveSpeed = 2.5f;
    public float sprintSpeed = 4f;
    private bool isSprinting = false;

    private bool isSwimming;
    [SerializeField] LayerMask waterMask;

    // Start is called before the first frame update
    void Start()
    {
        body = this.transform;
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // is the player trying to sprint?
        if(Input.GetButton("Sprint"))
            isSprinting = true;
        else
            isSprinting = false;

        // move the player
        movePlayer();
    }

    void movePlayer(){

        // get the axis input and assign appropriate names
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        float moveZ = Input.GetAxis("Forward");

        // takes the position the player is facing, creates a vector pointing in desired direction
        Vector3 move = transform.right * moveX + transform.up * moveY + camera.transform.forward * moveZ;

        controller.Move(move * (isSprinting ? sprintSpeed : moveSpeed) * Time.deltaTime);
    }
}
