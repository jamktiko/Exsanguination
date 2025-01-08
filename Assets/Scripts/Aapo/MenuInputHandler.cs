using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuInputHandler : MonoBehaviour
{
    [SerializeField] Button startButton;
    private ControllerHandler controllerHandler;
    private PlayerInput playerInput;
    private InputAction menuNavigateAction;
    [SerializeField] EventSystem eventSystem;

    [SerializeField] float inputCooldown = 0.2f; // Time delay between inputs
    private float lastInputTime = 0f;
    void Awake()
    {
        controllerHandler = GetComponent<ControllerHandler>();
        playerInput = GetComponent<PlayerInput>();

        menuNavigateAction = playerInput.actions["MenuNavigate"];

        menuNavigateAction.performed += ctx =>

        {
            if (controllerHandler.controllerIsConnected)
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                HandleMenuNavigation(input);

            }
        };
    }

    private void Start()
    {
        if (controllerHandler.controllerIsConnected)
        {
            eventSystem.firstSelectedGameObject = startButton.gameObject;
        }
    }


    private void NavigateMenu(Vector2 input)
    {

        // Get the currently selected object
        GameObject selectedObject = eventSystem.currentSelectedGameObject;


        // Use the Event System to navigate
        Selectable current = selectedObject.GetComponent<Selectable>();
        if (input.y > 0) // Navigate Up
        {
            Selectable next = current.FindSelectableOnUp();
            if (next != null)
            {
                eventSystem.SetSelectedGameObject(next.gameObject);
                Debug.Log("Navigate Up");
            }
        }
        else if (input.y < 0) // Navigate Down
        {
            Selectable next = current.FindSelectableOnDown();
            if (next != null)
            {
                eventSystem.SetSelectedGameObject(next.gameObject);
                Debug.Log("Navigate Down");
            }
        }
        else if (input.x > 0) // Navigate Right
        {
            Selectable next = current.FindSelectableOnRight();
            if (next != null)
            {
                eventSystem.SetSelectedGameObject(next.gameObject);
                Debug.Log("Navigate Right");
            }
        }
        else if (input.x < 0) // Navigate Left
        {
            Selectable next = current.FindSelectableOnLeft();
            if (next != null)
            {
                eventSystem.SetSelectedGameObject(next.gameObject);
                Debug.Log("Navigate Left");
            }
        }
    }

    private void HandleMenuNavigation(Vector2 navigationInput)
    {
        float currentTime = Time.time;
        if (currentTime - lastInputTime > inputCooldown)
        {
            lastInputTime = currentTime;
            NavigateMenu(navigationInput);
        }
    }

}
