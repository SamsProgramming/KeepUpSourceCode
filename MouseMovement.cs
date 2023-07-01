using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour

{

    public float mouseSens = 100.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()

    {

        // Locks the cursor in the game so that it doesn't move around when the player is moving the camera using the mouse.
        // This gets unlocked when the game is paused or when the player returns to menu.
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    
    {

        // Calls the move camera method.
        MoveCamera();

    }

    private void MoveCamera()

    {

        // If the camera being used is the main camera, i.e., the first person camera
        if (gameObject.CompareTag("MainCamera"))

        {

            // Note: Only the first person camera is moved by the mouse, the third person camera is static and the helicopter camera is moved automatically to
            // focus on the player.

            // Rotate it's x and y rotations to allow for mouse movement of the camera:
            // Rotate the X by getting the Y input of the mouse and multiplying it by -1 to inverse it so that it rotates in the same way as the mouse being moved.
            // This is further multiplied by the sensitivity of the mouse and by Time.deltaTime to keep it smoother.
            rotationX += Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime * -1;
            // Rotate the Y by getting the X input of the mouse, further multiplying it by the sensitivity and Time.deltaTime.
            rotationY += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;

            // Call the free look method which allows the mouse more freedom of movement if right click is held down.
            FreeLook();

            // Lastly, using the rotationX and rotationY, move the camera (but not the player's body) based on the mouse being moved around.
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

        }

    }

    private void FreeLook()

    {

        // If free look isn't engaged, i.e., the player doesn't hold right click, clamp the camera to certain angles:
        if (!Input.GetMouseButton(1))

        {

            // Lock the movement of the camera from left to right to 90 degrees
            rotationY = Mathf.Clamp(rotationY, -45, 45);

        }

        // If free look is engaged, still clamp the camera but this time from left to right to 300 degrees to allow more movement, but don't allow the camera to be able to move 360 degrees
        if (Input.GetMouseButton(1))

        {

            rotationY = Mathf.Clamp(rotationY, -150, 150);

        }

        // Moving the camera up and down is always locked to 120 degrees so that the camera doesn't rotate beyond natural angles and covers the camera clipping into the character model
        rotationX = Mathf.Clamp(rotationX, -60, 60);

    }

}