using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerAim : MonoBehaviour
{
    [SerializeField] float sensitivityX = 8f;       // Sensitivity for mouse X
    [SerializeField] float sensitivityY = 8f;       // Sensitivity for mouse Y (increased for smoother control)
    [SerializeField] Transform playerCamera;        // Reference to the player's camera
    [SerializeField] float xClamp = 85f;            // Clamp for X rotation
    private float xRotation = 0f;                   // Current X rotation (pitch)
    private float yRotation = 0f;                   // Current Y rotation (yaw)

    private Vector2 aimDirection;                    // Direction from the right stick
    private ControllerHandler controllerHandler;     // Reference to the ControllerHandler
    float mouseX, mouseY;
    private MouseLook mouseLook;
    private void Awake()
    {
        // Find the ControllerHandler in the scene
        controllerHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ControllerHandler>();
        Cursor.visible = false;                       // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;    // Lock cursor to the center of the screen
        mouseLook = GetComponent<MouseLook>();
    }

    private void Update()
    {
       

        // Handle gamepad input for camera rotation
        HandleControllerAim();
    }



    private void HandleControllerAim()
    {
        if (controllerHandler.controllerIsConnected)
        {
            mouseLook.enabled = false;

            // Get the direction of the right stick
            aimDirection = Gamepad.current.rightStick.ReadValue();

            // Set a deadzone threshold
            float deadzoneThreshold = 0.1f; // You can adjust this value as needed

            // Check if the aim direction is above the deadzone
            if (aimDirection.magnitude > deadzoneThreshold)
            {
                // Update yaw based on right stick X (horizontal)
                yRotation += aimDirection.x * sensitivityX * Time.deltaTime;

                // Update pitch based on right stick Y (vertical)
                xRotation -= aimDirection.y * sensitivityY * Time.deltaTime;
                xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp); // Clamp pitch rotation

                // Apply the new rotations to the camera and player
                playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Apply pitch rotation to camera
                transform.rotation = Quaternion.Euler(0f, yRotation, 0f); // Apply yaw rotation to player
            }
        }
        else
        {
            mouseLook.enabled = true;
        }
    }


    public void ReceiveInput(Vector2 mouseInput)
    {
        // Set mouse input values based on input received
        mouseX = mouseInput.x; // No need for multiplier since we handle it in HandleMouseLook
        mouseY = mouseInput.y; // Same here
    }
}
