using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
     public float sensitivity = 100f;
    [SerializeField] private float verticalClamp = 90f;

    private float xRotation = 0f;
    private Vector2 mouseInput = Vector2.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Method to receive input from Input System action
    public void ReceiveInput(Vector2 input)
    {
        mouseInput = input;
    }

    private void Update()
    {
        // Calculate rotations based on input
        float mouseX = mouseInput.x * sensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * sensitivity * Time.deltaTime;

        // Vertical rotation for camera (clamped)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation for player body
        transform.Rotate(Vector3.up * mouseX);
    }
}
