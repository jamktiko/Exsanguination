using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerAim : MonoBehaviour
{
    [SerializeField] private float sensitivityX = 8f; // Sensitivity for mouse X
    [SerializeField] private float sensitivityY = 8f; // Sensitivity for mouse Y
    [SerializeField] private Transform playerCamera; // Reference to the player's camera
    [SerializeField] private float xClamp = 85f;     // Clamp for X rotation
    private float xRotation = 0f;                    // Current X rotation (pitch)
    private float yRotation = 0f;                    // Current Y rotation (yaw)


    private void Awake()
    {
        // Find the ControllerHandler in the scene
        Cursor.visible = false;                        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;     // Lock cursor to the center of the screen
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            HandleControllerAim();
        }
    }

    private void HandleControllerAim()
    {
        
            // Get the direction of the right stick
            Vector2 aimDirection = Gamepad.current.rightStick.ReadValue();

            // Set a deadzone threshold
            float deadzoneThreshold = 0.2f; // Adjust this value as needed

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
}
