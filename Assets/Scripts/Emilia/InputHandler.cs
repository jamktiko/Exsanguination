using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GrapplingHookShoot grapplingHookShoot;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private StakeLogic stakeLogic;
    [SerializeField] private ThrowBomb throwBomb;
    [SerializeField] private PauseScript pauseScript;
    [SerializeField] private ControllerMenuNavigation menuNavigation;
    private PlayerInput playerInput;
    private InputAction movementAction, jumpAction, dashAction, slideAction, attackAction, grapplingAction, stakeAction, useAction, blockAction, throwableAction, pauseAction, weapon1Action, weapon2Action, pointAction, menuInteractionAction, menuNavigateAction, lookXAction, lookYAction;
    private Vector2 horizontalInput;
    private Vector2 mouseInput;
    private Vector2 cursorPosition;

    private bool stakeHoldDown;
    public bool inputsEnabled;
    private float stakeButtonDownTimer = 0f;
    public bool openDoor;
    private bool canAttack = true;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mouseLook = GetComponent<MouseLook>();

        movementAction = playerInput.actions["HorizontalMovement"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
        slideAction = playerInput.actions["Slide"];
        attackAction = playerInput.actions["Attack"];
        grapplingAction = playerInput.actions["Grapple"];
        stakeAction = playerInput.actions["Stake"];
        useAction = playerInput.actions["Use"];
        blockAction = playerInput.actions["Block"];
        throwableAction = playerInput.actions["Throwable"];
        pauseAction = playerInput.actions["Pause"];
        weapon1Action = playerInput.actions["Weapon1"];
        weapon2Action = playerInput.actions["Weapon2"];
        pointAction = playerInput.actions["Point"];
        menuInteractionAction = playerInput.actions["MenuInteraction"];
        menuNavigateAction = playerInput.actions["MenuNavigate"];
        lookXAction = playerInput.actions["MouseX"];
        lookYAction = playerInput.actions["MouseY"];



        pauseAction.performed += ctx =>
        {
            if (inputsEnabled)
            {
                if (!pauseScript.paused)
                {
                    pauseScript.PauseGame();
                    menuNavigation.SelectFirstMenuButton();

                }
                
            }

            else
            {
                if (pauseScript.paused)
                {
                    pauseScript.UnPauseGame();
                }
            }
        };

        menuNavigateAction.performed += ctx =>

        {
            if (pauseScript.paused)
            {
                Vector2 navigationInput = ctx.ReadValue<Vector2>();
                menuNavigation.Navigate(navigationInput);

            }
        };


        movementAction.performed += ctx =>
        {
            if (inputsEnabled)
            {
                horizontalInput = ctx.ReadValue<Vector2>();
                HorizontalInputCheck(ctx);

            }

        };

        pointAction.performed += ctx =>
        {
            if (inputsEnabled)
            {
                cursorPosition = ctx.ReadValue<Vector2>();
            }
        };

        jumpAction.performed += ctx =>
        {
            if (inputsEnabled)
                playerMovement.OnJumpPressed();
        };

        lookXAction.performed += ctx =>
        {
            if (inputsEnabled)
                mouseInput.x = ctx.ReadValue<float>();
        };

        lookYAction.performed += ctx =>
        {
            if (inputsEnabled)
                mouseInput.y = ctx.ReadValue<float>();
        };


        dashAction.performed += ctx =>
        {
            if (inputsEnabled)
                playerMovement.OnDashPressed();
        };

        slideAction.performed += ctx =>
        {
            if (inputsEnabled)
                playerMovement.OnSlidePressed();
        };

        attackAction.performed += ctx =>
        {
            if (inputsEnabled && canAttack)
                playerCombat.Attack();
        };

        blockAction.performed += ctx =>
        {
            if (inputsEnabled)
                playerCombat.BlockAction();
        };

        grapplingAction.performed += ctx =>
        {
            if (inputsEnabled)
                grapplingHookShoot.StartGrapple();
        };

        stakeAction.performed += ctx =>
        {
            if (inputsEnabled)
            {
                stakeHoldDown = true;
                canAttack = false;
                stakeLogic.StartThrowingChargingVisual();
            }
        };

        stakeAction.canceled += ctx =>
        {
            if (inputsEnabled)
            {
                stakeHoldDown = false;
                stakeLogic.StartThrowVisual();
                stakeLogic.ThrowStake(stakeButtonDownTimer);
                stakeButtonDownTimer = 0f;
                canAttack = true;
            }

        };

        useAction.performed += ctx =>
        {
            if (inputsEnabled)
            {
                stakeLogic.RetrieveStake();
                openDoor = true;
                Debug.Log("Open door = true");
            }
        };

        useAction.canceled += ctx =>
        {
            if (inputsEnabled)
                openDoor = false;
            Debug.Log("Open door = false");
        };

        throwableAction.performed += ctx =>
        {
            if (inputsEnabled)
                throwBomb.Throw();
        };

        weapon1Action.performed += ctx =>
        {
            if (inputsEnabled)
                playerCombat.SetWeaponLogics(0);
        };

        weapon2Action.performed += ctx =>
        {
            if (inputsEnabled)
                playerCombat.SetWeaponLogics(1);
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

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            pauseScript.PauseGame();
    }

    private void OnEnable()
    {
        EnableInput();

    }

    private void OnDestroy()
    {
        DisableInput();
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


    public void EnableInput()
    {
        inputsEnabled = true;
    }

    public void DisableInput() 
    {
        inputsEnabled = false;
    }

}

