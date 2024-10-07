using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GrapplingHookShoot grapplingHookShoot;
    [SerializeField] StarterSword starterSword;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] StakeLogic stakeLogic;
    [SerializeField] ThrowBomb throwBomb;
    [SerializeField] DoorFunctions doorFunctions;

    PlayerControls controls;
    PlayerControls.MovementActions movement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private bool stakeHoldDown;
    public bool inputsEnabled;
    private float stakeButtonDownTimer = 0f;

    private void Awake()
    {
        controls = new PlayerControls();
        movement = controls.Movement;

        movement.HorizontalMovement.performed += ctx =>
        {
            if (inputsEnabled)
                horizontalInput = ctx.ReadValue<Vector2>();
        };
        movement.HorizontalMovement.performed += HorizontalInputCheck;

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
            if (inputsEnabled)
                starterSword.ContinueCombo();
        };

        movement.Block.performed += ctx =>
        {
            if (inputsEnabled)
                starterSword.BlockAction();
        };



        movement.GrapplingHook.performed += ctx =>
        {
            if (inputsEnabled)
                grapplingHookShoot.StartGrapple();
        };

        movement.Stake.performed += ctx =>
        {
            if (inputsEnabled)
                stakeHoldDown = true;

            //enable stake and start visual
        };

        movement.Stake.canceled += ctx => {
            stakeHoldDown = false;
            stakeLogic.ThrowStake(stakeButtonDownTimer);
            stakeButtonDownTimer = 0f;
        };

        movement.Use.performed += ctx =>
        {
            if (inputsEnabled)
                stakeLogic.RetrieveStake();
            doorFunctions.OnOpenDoorPressed();
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
        if (movementInput != Vector2.zero)
        {
            playerMovement.isMoving = true;
        }
        else
        {
            playerMovement.isMoving = false;

        }
    }

    public Vector2 GetHorizontalInput()
    {
        return horizontalInput;
    }

}
