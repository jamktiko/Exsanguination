using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBPlayerMovement : MonoBehaviour
{
    // WASD Movement
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool canMove;
    public bool isMoving;
    Vector2 horizontalInput;
    public bool freeze;

    // Jump
    [SerializeField] float jumpForce;
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
    [SerializeField] bool canDash;
    [SerializeField] bool isDashing;
    Transform forwardT;

    // Slide
    [SerializeField] float slideSpeed;
    [SerializeField] float slideTime;
    float slideCooldownTimer;
    [SerializeField] CapsuleCollider playerCollider;
    [SerializeField] float playerColliderHeight;
    [SerializeField] bool canSlide;
    public bool isSliding;
    [SerializeField] Transform cam;
    [SerializeField] Vector3 camStandingPos;
    [SerializeField] Vector3 camSlidingPos;
    [SerializeField] Transform playerModel;
    [SerializeField] Vector3 playerModelStandingPos;
    [SerializeField] Vector3 playerModelSlidingPos;

    private void Start()
    {
        canMove = true;
        canDash = true;
        canSlide = true;
        dashDirection = orientation.forward;
    }

    private void Update()
    {
        if (freeze)
        {
            rb.velocity = Vector3.zero;
            canDash = false;
            canSlide = false;
            
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        if (isGrounded)
        {
            rb.drag = groundDrag;
            verticalVelocity.y = 0;
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.drag = airDrag;
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        SpeedControl();
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

        if (canDash)
        {
            canMove = false;
            StartCoroutine(DashCoroutine());
        }
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
            canDash = false;
            canSlide = true;
            cam.localPosition = camSlidingPos;
            StartCoroutine(SlideCoroutine());
        }
    }

    IEnumerator SlideCoroutine()
    {
        float startTime = Time.time;

        while (Time.time < startTime + slideTime)
        {
            isSliding = true;
            playerCollider.height = playerColliderHeight/2;
            playerCollider.center = new Vector3(0, -0.5f, 0.1f);
            playerModel.localPosition = playerModelSlidingPos;

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
        canDash = true;
        canSlide = true;
        isSliding = false;
        isDashing = false;
        slideSpeed = moveSpeed;
        cam.localPosition = camStandingPos;
        playerCollider.height = playerColliderHeight;
        playerCollider.center = new Vector3(0, 0, 0.1f);
        playerModel.localPosition = playerModelStandingPos;

    }
}
