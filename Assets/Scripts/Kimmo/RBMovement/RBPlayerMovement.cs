using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBPlayerMovement : MonoBehaviour
{
    // WASD Movement
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask groundMask;
    bool canMove;
    public bool isMoving;
    Vector2 horizontalInput;
    
    // Jump
    Vector3 verticalVelocity = Vector3.zero;
    public bool isGrounded;
    [SerializeField] bool isJumping;

    // Dash
    public Transform orientation;
    [SerializeField] Transform groundCheck;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    float dashCooldownTimer;
    [SerializeField] bool isDashing;

    // Slide
    [SerializeField] float slideSpeed;
    [SerializeField] float slideTime;
    float slideCooldownTimer;
    [SerializeField] bool canSlide;
    [SerializeField] bool isSliding;
    [SerializeField] Transform cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canMove = true;
        canSlide = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        if (isGrounded)
        {
            rb.drag = groundDrag;
            verticalVelocity.y = 0;
        }

        //verticalVelocity.y += gravity * Time.deltaTime;
        //controller.Move(verticalVelocity * Time.deltaTime);

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }

        if (isJumping)
        {
            Jump();
        }

        if (isDashing)
        {
            Dash();
        }

        if (isSliding)
        {
            Slide();
        }
    }

    private void Move()
    {
        Vector3 moveDirection = transform.right * horizontalInput.x + transform.forward * horizontalInput.y;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void Jump()
    {
        
        if (isGrounded)
        {
            //verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        isJumping = false;
    }

    private void Dash()
    {
        Transform forwardT;
        forwardT = orientation;
        Vector3 direction = GetDirection(forwardT);
        rb.AddForce(direction * dashSpeed, ForceMode.Impulse);
        //controller.Move(forceToApply * Time.deltaTime);
    }

    private void Slide()
    {
        slideSpeed = moveSpeed * 1.5f;
        rb.AddForce(orientation.forward * slideSpeed * 10f, ForceMode.Force);
        //Vector3 forceToApply = orientation.forward * slideSpeed;
        //controller.Move(forceToApply * Time.deltaTime);
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

    IEnumerator DashCoroutine()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            isDashing = true;
            
            yield return null;
        }
        OnCoroutineStopped();
    }

    public void OnSlidePressed()
    {
        if (canSlide && isMoving)
        {
            canMove = false;
            canSlide = true;
            //controller.height = 1f;
            //controller.center = new Vector3(0,-0.5f,0);
            cam.localPosition = new Vector3(0, 0.5f, 0.25f);
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
        //controller.height = 2f;
        //controller.center = Vector3.zero;
        cam.localPosition = new Vector3(0, 1.5f, 0.25f);
    }
}
