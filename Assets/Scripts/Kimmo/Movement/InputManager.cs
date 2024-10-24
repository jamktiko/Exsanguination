using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GrapplingHookShoot grapplingHookShoot;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private StakeLogic stakeLogic;
    [SerializeField] private ThrowBomb throwBomb;

    private PlayerControls controls;
    private PlayerControls.MovementActions movement;
    private InputAction slideInput;
    private Vector2 horizontalInput;
    private Vector2 mouseInput;

    private bool stakeHoldDown;
    public bool inputsEnabled;
    private float stakeButtonDownTimer = 0f;
    public bool openDoor;
    private bool canAttack = true;

    private void Awake()
    {
        controls = new PlayerControls();
        movement = controls.Movement;

        movement.HorizontalMovement.performed += ctx =>
        {
            if (inputsEnabled)
                horizontalInput = ctx.ReadValue<Vector2>();
            HorizontalInputCheck(ctx);
        };

        movement.Jump.performed += ctx =>
        {
            if (inputsEnabled)
                playerMovement.OnJumpPressed();
        };

        movement.MouseX.performed += ctx =>
        {
            if (inputsEnabled)
                mouseInput.x = ctx.ReadValue<float>();
        };

        movement.MouseY.performed += ctx =>
        {
            if (inputsEnabled)
                mouseInput.y = ctx.ReadValue<float>();
        };

        movement.Dash.performed += ctx =>
        {
            if (inputsEnabled)
                playerMovement.OnDashPressed();
        };

        movement.Slide.performed += ctx =>
        {
            if (inputsEnabled)
                playerMovement.OnSlidePressed();
        };
        
        movement.Attack.performed += ctx =>
        {
            if (inputsEnabled && canAttack)
                playerCombat.Attack();
        };

        movement.Block.performed += ctx =>
        {
            if (inputsEnabled)
                playerCombat.BlockAction();
        };

        movement.GrapplingHook.performed += ctx =>
        {
            if (inputsEnabled)
                grapplingHookShoot.StartGrapple();
        };

        movement.Stake.performed += ctx =>
        {
            if (inputsEnabled)
            {
                stakeHoldDown = true;
                canAttack = false;
                stakeLogic.StartThrowingChargingVisual();
            }
        };

        movement.Stake.canceled += ctx =>
        {
            stakeHoldDown = false;
            stakeLogic.StartThrowVisual();
            stakeLogic.ThrowStake(stakeButtonDownTimer);
            stakeButtonDownTimer = 0f;
            canAttack = true;
        };

        movement.Use.performed += ctx =>
        {
            if (inputsEnabled)
            {
                stakeLogic.RetrieveStake();
                openDoor = true;
                Debug.Log("Open door = true");
            }
        };

        movement.Use.canceled += ctx =>
        {
            if (inputsEnabled)
                openDoor = false;
            Debug.Log("Open door = false");
        };

        movement.SilverBomb.performed += ctx =>
        {
            if (inputsEnabled)
                throwBomb.Throw();
        };
    }

    private void Start()
    {
        inputsEnabled = true;
    }

    private void Update()
    {
        playerMovement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);

        if (stakeHoldDown)
        {
            stakeButtonDownTimer += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }

    private void HorizontalInputCheck(InputAction.CallbackContext ctx)
    {
        Vector2 movementInput = ctx.ReadValue<Vector2>();
        playerMovement.isMoving = movementInput != Vector2.zero;
    }

    public Vector2 GetHorizontalInput()
    {
        return horizontalInput;
    }
}
