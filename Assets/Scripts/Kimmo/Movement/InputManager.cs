using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GrapplingHookShoot grapplingHookShoot;
    [SerializeField] StarterSword aapoSwordSwing;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] StakeLogic stakeLogic;
    [SerializeField] ThrowBomb throwBomb;

    PlayerControls controls;
    PlayerControls.MovementActions movement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private bool stakeHoldDown;
    private float stakeButtonDownTimer = 0f;

    private void Awake()
    {
        controls = new PlayerControls();
        movement = controls.Movement;
        
        movement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        movement.HorizontalMovement.performed += HorizontalInputCheck;

        movement.Jump.performed += ctx => playerMovement.OnJumpPressed();

        movement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        movement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        movement.Dash.performed += ctx => playerMovement.OnDashPressed();

        movement.Slide.performed += ctx => playerMovement.OnSlidePressed();

        movement.Attack.performed += ctx => aapoSwordSwing.ContinueCombo();

        movement.Block.performed += ctx => aapoSwordSwing.BlockAction();

        movement.GrapplingHook.performed += ctx => grapplingHookShoot.StartGrapple();

        movement.Stake.performed += ctx => stakeHoldDown = true;
        movement.Stake.canceled += ctx => {
            stakeHoldDown = false;
            stakeLogic.ThrowStake(stakeButtonDownTimer);
            stakeButtonDownTimer = 0f;
        };
        movement.Use.performed += ctx => stakeLogic.RetrieveStake();

        movement.SilverBomb.performed += ctx => throwBomb.Throw();

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

   public void ControlsEnabled(bool enabledControls)
    {
        if (enabledControls)
        {
            Debug.Log("controls enabled");
            controls.Enable();
        }
        else
        {
            Debug.Log("controls disabled");
            controls.Disable();
        }
    }



    private void HorizontalInputCheck(InputAction.CallbackContext ctx)
    {
        Vector2 movementInput = ctx.ReadValue < Vector2>();
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
