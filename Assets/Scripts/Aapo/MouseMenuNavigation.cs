using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseMenuNavigation : MonoBehaviour
{
    private EventSystem eventSystem;
    private Selectable previousSelectable; // To track the previously selected button
    [SerializeField] private PauseScript pauseScript;
    [SerializeField] DeathScript deathScript;
    private ControllerHandler controllerHandler;

    private void Awake()
    {
        controllerHandler = GetComponent<ControllerHandler>();
    }

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (pauseScript.paused || deathScript.isDead)
        {
            // Check if the pointer is over a UI element
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            // Create a list to store results from Raycast
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            // If there's a UI element under the mouse, select it
            if (raycastResults.Count > 0)
            {
                GameObject hoveredObject = raycastResults[0].gameObject;
                Selectable hoveredSelectable = hoveredObject.GetComponent<Selectable>();

                // Check if the hovered object is a selectable
                if (hoveredSelectable != null)
                {
                    // Set the hovered button as the selected game object
                    eventSystem.SetSelectedGameObject(hoveredObject);
                }
            }
            else
            {
                // If no UI elements are hovered, clear selection and reset highlight
                eventSystem.SetSelectedGameObject(null);
                if (previousSelectable != null)
                {
                    previousSelectable = null; // Clear the previous reference
                }
            }
        }
    }   
}
