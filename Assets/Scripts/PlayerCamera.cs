using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private float mouseSens = 250.0f;     // to be set externally by player
    public Transform playerBody;
    float xRotate = 0.0f;               // the rotation to be applied to the player
    int inverted = -1;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        changeInversion(false);
    }

    // Update is called once per frame
    void Update()
    {
        // get the location of the mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        // clamp the rotation to prevent the player from looked too far up or down
        xRotate += mouseY * inverted;
        xRotate = Mathf.Clamp(xRotate, -135.0f, 135.0f);

        // rotate camera and body
        transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // helper function allows for sensitivity to be set
    void changeSens(float newSens) {
        mouseSens = newSens;
    }

    // this function allows for inverted camera control if player wishes
    void changeInversion(bool isInvert) {
        inverted = isInvert ? 1 : -1;
    }
}
