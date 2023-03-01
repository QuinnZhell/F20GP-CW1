using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class PlayerMovement : MonoBehaviour
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

    [Header ("Grounded Behaviour")]
    [SerializeField] bool isGrounded;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject groundCheck;
    [SerializeField] Vector3 velocity;
    [SerializeField] float gravity = -6f;
    [SerializeField] float jumpHeight = 1f;

    // Start is called before the first frame update
    void Start()
    {
        body = this.transform;
        isSwimming = false;
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if(!isSwimming) {
            PerformGroundCheck();

            if(Input.GetButtonDown("Jump") && isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        // is the player trying to sprint?
        if(Input.GetButton("Sprint"))
            isSprinting = true;
        else
            isSprinting = false;

        // move the player
        movePlayer();
    }

    void PerformGroundCheck() {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);
        Debug.Log(isGrounded);
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
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
