using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // WASD Movement
    [SerializeField] CharacterController controller;
    [SerializeField] float moveSpeed;
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;
    [SerializeField] LayerMask groundMask;
    bool canMove;
    public bool isMoving;
    Vector2 horizontalInput;
    
    // Jump
    Vector3 verticalVelocity = Vector3.zero;
    public bool isGrounded;
    bool isJumping;

    // Dash
    public Transform orientation;
    [SerializeField] Transform groundCheck;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    float dashCooldownTimer;

    // Slide
    [SerializeField] float slideSpeed;
    [SerializeField] float slideTime;
    float slideCooldownTimer;
    [SerializeField] bool canSlide;
    [SerializeField] Transform cam;

    private void Start()
    {
        canMove = true;
        canSlide = true;
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
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }

            isJumping = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
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
        StartCoroutine(Dash());
        canMove = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();
        direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;

        if (verticalInput == 0 && horizontalInput == 0)
        {
            direction = forwardT.forward;
        }

        return direction.normalized;
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        Transform forwardT;
        forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);

        while (Time.time < startTime + dashTime)
        {
            Vector3 forceToApply = direction * dashSpeed;
            controller.Move(forceToApply * Time.deltaTime);
            
            yield return null;
        }
    }

    public void OnSlidePressed()
    {
        if (canSlide && isMoving)
        {
            canMove = false;
            controller.height = 1f;
            controller.center = new Vector3(0,-0.5f,0);
            cam.localPosition = new Vector3(0, 0, 0.5f);
            StartCoroutine(Slide());
        }
    }

    IEnumerator Slide()
    {
        float startTime = Time.time;

        slideSpeed = moveSpeed * 1.5f;

        while (Time.time < startTime + slideTime)
        {
            canSlide = false;
            Vector3 forceToApply = orientation.forward * slideSpeed;
            controller.Move(forceToApply * Time.deltaTime);

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
        slideSpeed = moveSpeed;
        controller.height = 2f;
        controller.center = Vector3.zero;
        cam.localPosition = new Vector3(0, 0.5f, 0.5f);
    }
}
