using UnityEngine;

public class AapoDash : MonoBehaviour
{
    private CharacterController characterController;

    // Dash settings
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    private float dashCooldown = 1f;
    public bool canDash = true;

    // Regular movement settings
    public float moveSpeed = 5f;
    private Vector3 moveDirection;

    // Dash transition control
    private bool isDashing = false;
    private float currentDashTime;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isDashing)
        {
            HandleMovement();

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartDash();
            }
        }
        else
        {
            PerformDash();
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void StartDash()
    {
        isDashing = true;
        currentDashTime = 0f;
        canDash = false;
    }

    void PerformDash()
    {
        currentDashTime += Time.deltaTime;

        if (currentDashTime < dashTime)
        {
            float dashProgress = currentDashTime / dashTime;
            Vector3 dashVelocity = moveDirection * dashSpeed * (1f - dashProgress); // Smooth transition
            characterController.Move(dashVelocity * Time.deltaTime);
        }
        else
        {
            isDashing = false;
            Invoke("ResetDash", dashCooldown); // Cooldown before next dash
        }
    }

    void ResetDash()
    {
        canDash = true;
    }
}
