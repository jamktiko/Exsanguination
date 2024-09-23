using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RBInputManager : MonoBehaviour
{
    [SerializeField] RBPlayerMovement playerMovement;
    [SerializeField] Grappling grappling;
    public AapoSwordSwing aapoSwordSwing;
    [SerializeField] MouseLook mouseLook;

    PlayerControls controls;
    PlayerControls.MovementActions movement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        controls = new PlayerControls();
        movement = controls.Movement;
        
        movement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        movement.HorizontalMovement.performed += HorizontalInputCheck;
        movement.HorizontalMovement.performed += ctx => playerMovement.GetDirection();

        movement.Jump.performed += ctx => playerMovement.OnJumpPressed();

        movement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        movement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        movement.Dash.performed += ctx => playerMovement.OnDashPressed();

        movement.Slide.performed += ctx => playerMovement.OnSlidePressed();

        movement.Attack.started += ctx => aapoSwordSwing.SwingSword();
        movement.Attack.performed += ctx => aapoSwordSwing.ContinueCombo();
        movement.Attack.canceled += ctx => aapoSwordSwing.StopCombo();

        movement.GrapplingHook.performed += ctx => grappling.StartGrapple();

    }

    private void Update()
    {
        playerMovement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
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
}
