using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float airMultiplier;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float grapplingDrag;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool canMove;
    public bool isMoving;
    Vector2 horizontalInput;
    Rigidbody rb;
    private Animator animator;
    StakeLogic stakeLogic;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    public bool isGrounded;
    [SerializeField] bool isJumping;
    bool canJump;
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] float coyoteTime;
    float coyoteTimeCounter;

    [Header("Dash")]
    [SerializeField] Vector3 dashDirection;
    [SerializeField] Transform groundCheck;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashCooldownTimer;
    [SerializeField] bool canDash;
    [SerializeField] bool isDashing;
    [SerializeField] Transform orientation;
    Transform forwardT;

    [Header("Slide")]
    [SerializeField] float slideSpeed;
    [SerializeField] float slideSpeedMultiplier;
    [SerializeField] float slideTime;
    [SerializeField] float slideCooldown;
    [SerializeField] float slideCooldownTimer;
    [SerializeField] CapsuleCollider playerCollider;
    [SerializeField] float playerColliderHeight;
    [SerializeField] bool canSlide;
    public bool isSliding;
    [SerializeField] bool isOnWall;
    [SerializeField] Transform cam;
    [SerializeField] Vector3 camStandingPos;
    [SerializeField] Vector3 camSlidingPos;
    [SerializeField] Transform playerModel;
    [SerializeField] Vector3 playerModelStandingPos;
    [SerializeField] Vector3 playerModelSlidingPos;

    [Header("Grapple")]
    public bool activeGrapple;
    GrapplingHookShoot grapplingHookShoot;

    [Header("UI")]
    CooldownUI cooldownUI;

    [Header("Audio")]
    [SerializeField] float footstepTimer;
    [SerializeField] float footStepAudioCooldown;
    bool firstStepPlayed;
    bool isLanded;
    AudioManager audioManager;

    [Header("Controller")]
    private ControllerHandler controllerHandler;
    private Vector2 movementInput; // Changed from Vector3 to Vector2

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        dashDirection = orientation.forward;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        controllerHandler = GameObject.FindGameObjectWithTag("InputManager").GetComponent<ControllerHandler>();
        animator = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();
        grapplingHookShoot = GetComponent<GrapplingHookShoot>();
        cooldownUI = GameObject.Find("DashCooldownBar").GetComponent<CooldownUI>();
        stakeLogic = GameObject.FindGameObjectWithTag("Stake").GetComponent<StakeLogic>();
    }

    private void Start()
    {
        canMove = true;
        canJump = true;
        canDash = true;
        canSlide = true;
        isLanded = true;
        dashCooldownTimer = dashCooldown;
        slideCooldownTimer = slideCooldown;

        cooldownUI.SetDashCooldownMaxValue(dashCooldown);
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        if (isGrounded) 
        {
            if (!isLanded)
            {
                audioManager.PlayPlayerLandAudioClip();
                isLanded = true;
            }

            if (!activeGrapple)
            {
                verticalVelocity.y = 0;
            }

            coyoteTimeCounter = 0;
        }

        if (!isGrounded)
        {
            isLanded = false;
            coyoteTimeCounter += Time.deltaTime;
        }

        if (rb.velocity.y < 0)
        {
            rb.drag = airDrag;
        }

        if (isGrounded || isJumping)
        {
            rb.drag = groundDrag;
        }

        if (activeGrapple)
        {
            rb.drag = grapplingDrag;
        }

        if (coyoteTimeCounter < coyoteTime)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if (dashCooldownTimer < dashCooldown)
        {
            dashCooldownTimer += Time.deltaTime;
            cooldownUI.UpdateDashCooldownBar(dashCooldownTimer);
        }

        if (slideCooldownTimer < slideCooldown)
        {
            slideCooldownTimer += Time.deltaTime;
        }

        if (isMoving && isGrounded)
        {
            if (rb.velocity != Vector3.zero)
            {
                footstepTimer += Time.deltaTime;

                if (footstepTimer >= footStepAudioCooldown)
                {
                    audioManager.PlayPlayerFootstepsAudioClips();
                    footstepTimer = 0f;
                }
            }
        }

        SpeedControl();
        GetDirection();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (controllerHandler.controllerIsConnected) 
            {
                ControllerMovement();
            }
            else
            {
            Move();
            }

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
    private void ControllerMovement()
    {
        if (activeGrapple) return;

        // Read left stick input from the gamepad
        movementInput = Gamepad.current.leftStick.ReadValue();

        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
        moveDirection = transform.TransformDirection(moveDirection); // Convert to world space
        moveDirection.y = 0; // Keep movement strictly horizontal

        // Directly set the velocity of the Rigidbody
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // Optional: Clamp horizontal speed to ensure max speed is maintained
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            isMoving = true;
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }

        //if (isGrounded)
        //{
        //    rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        //}
        //else if (!isJumping)
        //{
        //    rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        //}

    }

    // Movement methods
    private void Move()
    {
        if (activeGrapple) return;

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
        if (activeGrapple) return;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            isOnWall = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            isOnWall = false;
        }
    }

    // Jump methods
    public void OnJumpPressed()
    {
        if (canJump)
        {
            isJumping = true;
        }
    }

    private void Jump()
    {
        coyoteTimeCounter = coyoteTime;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        audioManager.PlayPlayerJumpAudioClip();
        isJumping = false;
    }

    // Dash methods
    public void OnDashPressed()
    {
        if (dashCooldownTimer < dashCooldown || !isMoving || !canDash) return;
        else dashCooldownTimer = 0;

        //audioManager.PlayDashAudioClip();
        canMove = false;
        canSlide = false;
        StartCoroutine(DashCoroutine());
    }

    private void Dash()
    {
        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        
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
        audioManager.PlayDashAudioClip();
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            isDashing = true;
            canMove = false;
            canSlide = false;
            
            yield return null;
        }

        OnCoroutineStopped();
    }

    // Slide methods
    public void OnSlidePressed()
    {
        if (slideCooldownTimer < slideCooldown || !isMoving || !canSlide) return;
        else slideCooldownTimer = 0;

        if (isGrounded || isOnWall)
        {
            canMove = false;
            canDash = false;
            canSlide = false;
            cam.localPosition = camSlidingPos;
            //audioManager.PlaySlideAudioClip();
            StartCoroutine(SlideCoroutine());
        }
    }

    private void Slide()
    {
        slideSpeed = moveSpeed * slideSpeedMultiplier;
        rb.AddForce(orientation.forward * slideSpeed * 10f, ForceMode.Force);
        if (isOnWall)
        {
            rb.AddForce(orientation.up * 9.81f, ForceMode.Force);
        }
    }

    IEnumerator SlideCoroutine()
    {
        audioManager.PlaySlideAudioClip();
        float startTime = Time.time;
        animator.SetTrigger("slide");
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

    // End of coroutine method after dash and slide
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

    // Grapple methods
    private bool enableMovementOnNextTouch;

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        canDash = false;
        canJump = false;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestricitons), grapplingHookShoot.grapplingCd);
    }

    private Vector3 velocityToSet;

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }

    public void ResetRestricitons()
    {
        activeGrapple = false;
        canDash = true;
        canJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;

            if (grapplingHookShoot.isGrappling)
            {
                grapplingHookShoot.StopGrapple();
            }
        }
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    // Audio methods
    IEnumerator WaitBeforeAudioClip()
    {
        audioManager.PlayPlayerFootstepsAudioClips();
        yield return new WaitForSeconds(1);
        Debug.Log("Footstep audio is playing.");
    }

    public void EnableMovement()
    {
        canMove = false;
        canJump = false;
        canDash = false;
        canSlide = false;
    }

    public void DisableMovement()
    {
        canMove = true;
        canJump = true;
        canDash = true;
        canSlide = true;
    }
}
