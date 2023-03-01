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
    public float moveSpeed;
    public float sprintSpeed;
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
        isSwimming = true;
        controller = this.GetComponent<CharacterController>();
        moveSpeed = 5f;
        sprintSpeed = 8f;
    }

    // Update is called once per frame
    void Update()
    {

        // player has two states, swimming and walking
        // these are the walking behaviours
        if(!isSwimming) {
            // check the player is touching the ground
            PerformGroundCheck();

            // if grounded they are able to jump
            if(Input.GetButtonDown("Jump") && isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // apply gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        // player should slightly sink underwater
        if(isSwimming) {
            velocity.y = -0.5f;
            controller.Move(velocity * Time.deltaTime);
            
        }

        // is the player trying to sprint?
        if(Input.GetButton("Sprint"))
            isSprinting = true;
        else
            isSprinting = false;

        // move the player
        MovePlayer();
    }

    // checks that the player is in contact with the floor
    void PerformGroundCheck() {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);
        Debug.Log(isGrounded);

        // make sure the players velocity doesn't not continually decrease due to gravity
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
    }

    void MovePlayer(){

        // get the axis input and assign appropriate names
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        float moveZ = Input.GetAxis("Forward");

        // takes the position the player is facing, creates a vector pointing in desired direction
        Vector3 move = transform.right * moveX + transform.up * moveY + camera.transform.forward * moveZ;

        controller.Move(move * (isSprinting ? sprintSpeed : moveSpeed) * Time.deltaTime);
    }

    public void StartSwimming() {
        Debug.Log("I am swimming");
        
        isSwimming = true;
        RenderSettings.fogColor = new Color32(49,127,171,255);
        RenderSettings.fogDensity = 0.015f;
    }

    public void StartWalking() {
        Debug.Log("I am walking");

        isSwimming = false;
        RenderSettings.fogColor = new Color32(0,0,0,255);
        RenderSettings.fogDensity = 0.1f;
    }
}
