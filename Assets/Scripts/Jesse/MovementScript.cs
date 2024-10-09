using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float sensitivity;
    [SerializeField] Camera playerCamera;
    [SerializeField] StaminaBarScript staminaBarScript;
    [SerializeField] float jumpForce;

    Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        PlayerRotation();
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Jump();
        }
    }

    private void PlayerRotation()
    {
        float mouseAxisY = Input.GetAxis("Mouse Y");
        float mouseAxisX = Input.GetAxis("Mouse X");

        playerCamera.transform.Rotate(-mouseAxisY * sensitivity * Time.deltaTime, 0, 0);
        transform.Rotate(0, mouseAxisX * sensitivity * Time.deltaTime, 0);
    }

    private void PlayerMovement()
    {
        float axisX = Input.GetAxisRaw("Horizontal");
        float axisY = Input.GetAxisRaw("Vertical");

        rb.velocity = transform.forward * axisY * movementSpeed + transform.right * axisX * movementSpeed;
    }

    private void Jump()
    {
        if (staminaBarScript.GetStamina() >= 50f)
        {
            staminaBarScript.UseStamina(50);
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
        
    }
}
