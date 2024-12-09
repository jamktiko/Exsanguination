using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    public float sensitivity = 100f;
    [SerializeField] private float verticalClamp = 90f;
    [SerializeField] EnemyFinisher enemyFinisher;

    private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraTransform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    // Method to receive input from Input System action
    public void ReceiveInput(Vector2 input)
    {
        if (!enemyFinisher.isFinishing)
        {
            // Calculate rotations based on input
            float mouseX = input.x * sensitivity * Time.deltaTime;
            float mouseY = input.y * sensitivity * Time.deltaTime;

            // Vertical rotation for camera (clamped)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Horizontal rotation for player body
            transform.Rotate(Vector3.up * mouseX);
        }
        
    }
}
