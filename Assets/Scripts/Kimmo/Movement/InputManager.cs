using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GrapplingHookShoot grapplingHookShoot;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private StakeLogic stakeLogic;
    [SerializeField] private ThrowBomb throwBomb;
    [SerializeField] private PauseScript pauseScript;
    [SerializeField] private ControllerHandler controllerHandler;
    private PlayerControls controls;
    private PlayerControls.MovementActions movement;
    private Vector2 horizontalInput;
    private Vector2 mouseInput;

    private bool stakeHoldDown;
    public bool inputsEnabled;
    private float stakeButtonDownTimer = 0f;
    public bool openDoor;
    private bool canAttack = true;
    public EventSystem eventSystem;
    private Selectable previousSelectable; // To track the previously selected button
    [SerializeField] GameObject ContinueButton;
    private Selectable firstSelectable;

    private void Awake()
    {
        controls = new PlayerControls();
        movement = controls.Movement;
        movement.PauseMenu.performed += ctx =>
        {
            if (inputsEnabled)
            {
                if (!pauseScript.paused)
                {
                    pauseScript.PauseGame();
                }
                else
                {
                    pauseScript.UnPauseGame();
                }
            }
        };

        movement.MenuNavigate.performed += ctx =>
        {
            if (inputsEnabled)
            {
                Vector2 navigationInput = ctx.ReadValue<Vector2>();
                Navigate(navigationInput);

            }
        };
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
        if (!controllerHandler.controllerIsConnected)
        {
            firstSelectable = ContinueButton.GetComponent<Selectable>();
            HighlightButton(firstSelectable);
        }
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

    private void Navigate(Vector2 input)
    {
        // Get the currently selected GameObject
        GameObject currentlySelected = EventSystem.current.currentSelectedGameObject;

        Selectable newSelectable = null;


        if (input.y > 0) // Up input
        {
            // Move the selection to the previous button
            newSelectable = currentlySelected?.GetComponent<Selectable>()?.FindSelectableOnUp();
        }
        else if (input.y < 0) // Down input
        {
            // Move the selection to the next button
            newSelectable = currentlySelected?.GetComponent<Selectable>()?.FindSelectableOnDown();
        }

        if (newSelectable != null)
        {
            EventSystem.current.SetSelectedGameObject(newSelectable.gameObject);
            HighlightButton(newSelectable);
        }



    }
    private void HighlightButton(Selectable selectable)
    {
        // Reset the previous highlight
        if (previousSelectable != null)
        {
            ResetButtonHighlight(previousSelectable);
        }

        // Highlight the current button
        selectable.Select(); // This also sets it as the selected button

        var button = selectable.GetComponent<Button>();
        if (button != null)
        {
            ColorBlock colors = button.colors;
            colors.selectedColor = Color.yellow; // Change to your desired highlight color
            button.colors = colors;

            // Ensure the button visually reflects the new highlighted state
            button.OnSelect(null); // This forces the button to visually update to selected
        }

        previousSelectable = selectable; // Update the previously selected
    }

    // Reset the button highlight
    private void ResetButtonHighlight(Selectable selectable)
    {
        var button = selectable.GetComponent<Button>();
        if (button != null)
        {
            ColorBlock colors = button.colors;
            colors.selectedColor = Color.white; // Reset to default color
            button.colors = colors;

            button.OnDeselect(null); // Force button to visually update to deselected
        }
    }

   
}
