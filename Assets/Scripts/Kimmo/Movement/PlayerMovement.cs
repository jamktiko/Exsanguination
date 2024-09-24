using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // WASD Movement
    [SerializeField] CharacterController controller;
    [SerializeField] float moveSpeed;
    [SerializeField] float gravity;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool canMove;
    public bool isMoving;
    Vector2 horizontalInput;

    // Jump
    [SerializeField] float jumpForce;
    Vector3 verticalVelocity = Vector3.zero;
    public bool isGrounded;
    [SerializeField] bool isJumping;

    // Dash
    Vector3 dashDirection;
    public Transform orientation;
    [SerializeField] Transform groundCheck;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    float dashCooldownTimer;
    [SerializeField] bool isDashing;
    Transform forwardT;

    // Slide
    [SerializeField] float slideSpeed;
    [SerializeField] float slideTime;
    float slideCooldownTimer;
    [SerializeField] bool canSlide;
    [SerializeField] bool isSliding;
    [SerializeField] Transform cam;

    private void Start()
    {
        canMove = true;
        canSlide = true;
        dashDirection = orientation.forward;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }

        if (canMove)
        {
            Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * moveSpeed;
            controller.Move(horizontalVelocity * Time.deltaTime);
        }

        if (isJumping)
        {
            Jump();
        }

        if (isSliding)
        {
            Slide();
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (isDashing)
        {
            Dash();
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(-2f * jumpForce * gravity);
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        isJumping = false;
    }

    private void Dash()
    {
        Vector3 direction = dashDirection;
        Vector3 forceToApply = direction * dashSpeed;
        controller.Move(forceToApply * Time.deltaTime);
    }

    private void Slide()
    {
        slideSpeed = moveSpeed * 1.5f;
        Vector3 forceToApply = orientation.forward * slideSpeed;
        controller.Move(forceToApply * Time.deltaTime);
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void OnJumpPressed()
    {
        isJumping = true;
    }

    public void OnDashPressed()
    {
        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;

        canMove = false;
        StartCoroutine(DashCoroutine());
    }

    public void GetDirection()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        forwardT = orientation;

        Vector3 direction = new Vector3();

        direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;

        if (verticalInput == 0 && horizontalInput == 0 && !isDashing)
        {
            direction = forwardT.forward;
        }

        dashDirection = direction;
    }

    IEnumerator DashCoroutine()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            isDashing = true;
            canMove = false;

            yield return null;
        }

        OnCoroutineStopped();
    }

    public void OnSlidePressed()
    {
        if (canSlide && isMoving)
        {
            canMove = false;
            controller.height = 1f;
            controller.center = new Vector3(0,-0.5f,0);
            cam.localPosition = new Vector3(0, 0, 0.5f);
            StartCoroutine(SlideCoroutine());
        }
    }

    IEnumerator SlideCoroutine()
    {
        float startTime = Time.time;

        while (Time.time < startTime + slideTime)
        {
            isSliding = true;
            
            if (slideSpeed > moveSpeed)
            {
                slideSpeed -= Time.deltaTime * slideTime;
            }

            yield return null;
        }
        OnCoroutineStopped();
    }

    private void OnCoroutineStopped()
    {
        canMove = true;
        canSlide = true;
        isSliding = false;
        isDashing = false;
        slideSpeed = moveSpeed;
        controller.height = 2f;
        controller.center = Vector3.zero;
        cam.localPosition = new Vector3(0, 0.5f, 0.5f);
    }
}
