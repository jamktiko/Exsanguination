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
    [SerializeField] bool canMove;
    public bool isMoving;
    Vector2 horizontalInput;

    // Jump
    [SerializeField] float airMultiplier;
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
            rb.drag = groundDrag;
            verticalVelocity.y = 0;
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        SpeedControl();
        //GetLastDirection();
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            //rb.velocity += custom
        }

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

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        else if (!isJumping) 
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        isJumping = false;
    }

    private void Dash()
    {
        //Transform forwardT;
        //forwardT = orientation;

        //Vector3 direction = dashDirection;
        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);
    }

    private void Slide()
    {
        slideSpeed = moveSpeed * 1.5f;
        rb.AddForce(orientation.forward * slideSpeed * 10f, ForceMode.Force);
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

        //return direction.normalized;
    }

    //public IEnumerator DashDirectionTimerCoroutine()
    //{
    //    yield return new WaitForSeconds(1);
    //    Debug.Log("Blaa");

    //    if (!isDashing)
    //    {
    //        dashDirection = forwardT.forward;

    //    }
        
    //}

    //public void DashDirectionTimer()
    //{
    //    Debug.Log("Stopped to press button");
    //    StartCoroutine(DashDirectionTimerCoroutine());
    //}

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
            canSlide = true;
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
        cam.localPosition = new Vector3(0, 1.5f, 0.25f);
    }
}
