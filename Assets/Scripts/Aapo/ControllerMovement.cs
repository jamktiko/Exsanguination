using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerMovement : MonoBehaviour
{
    private ControllerHandler controllerHandler;
    private PlayerMovement playerMovement;
    private GrapplingHookShoot grapplingHookShoot;
    private PlayerCombat playerCombat;
    private StakeLogic stakeLogic;
    private InputManager inputManager;

    private bool stakeHoldDown;
    private float stakeButtonDownTimer = 0f;
    private bool canAttack = true;

    //private ThrowBomb throwBomb;

    private void Awake()
    {
        controllerHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ControllerHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        grapplingHookShoot = GetComponent<GrapplingHookShoot>();
        playerCombat = GetComponentInChildren<PlayerCombat>();
        stakeLogic = GetComponentInChildren<StakeLogic>();
        inputManager = GetComponent<InputManager>();
        // throwBomb = GetComponent<ThrowBomb>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (controllerHandler.controllerIsConnected)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                if (inputManager.inputsEnabled)
                    playerMovement.OnJumpPressed();
            }

            if (Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                if (inputManager.inputsEnabled)
                    stakeLogic.RetrieveStake();

            }

            if (Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                if (inputManager.inputsEnabled)
                    playerMovement.OnDashPressed();

            }

            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                if (inputManager.inputsEnabled)
                playerMovement.OnSlidePressed();

            }

            if (Gamepad.current.rightTrigger.wasPressedThisFrame)
            {
                if (inputManager.inputsEnabled)
                    playerCombat.Attack();

            }
            if (Gamepad.current.leftTrigger.wasPressedThisFrame)
            {
                if (inputManager.inputsEnabled)
                {
                    stakeHoldDown = true;
                    canAttack = false;
                    stakeLogic.StartThrowingChargingVisual();
                }

            }

            if (Gamepad.current.leftTrigger.wasReleasedThisFrame)
            {
                if (inputManager.inputsEnabled)
                {
                    stakeHoldDown = false;
                    stakeLogic.StartThrowVisual();
                    stakeLogic.ThrowStake(stakeButtonDownTimer);
                    stakeButtonDownTimer = 0f;
                    canAttack = true;
                }

            }

            if (Gamepad.current.leftShoulder.wasPressedThisFrame)
            {
                if (inputManager.inputsEnabled)
                    grapplingHookShoot.StartGrapple();
            }

            if (Gamepad.current.rightShoulder.wasPressedThisFrame)
            {
                if (inputManager.inputsEnabled)
                    playerCombat.BlockAction();
            }
        }
    }
}
