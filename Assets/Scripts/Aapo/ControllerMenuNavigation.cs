using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ControllerMenuNavigation : MonoBehaviour
{
    public EventSystem eventSystem; // Unity EventSystem for handling selections
    public Selectable firstButton; // The default button when the menu is opened
    public Selectable firstDeathButton; // The default button when the menu is opened
    public float inputCooldown = 0.2f;  // Cooldown duration in seconds
    private float lastInputTime = 0f;

    private void Start()
    {

        // Set the first selected button when the menu is opened
        if (firstButton != null)
        {
            eventSystem.SetSelectedGameObject(firstButton.gameObject);

        }
    }



    public void Navigate(Vector2 navigationInput)
    {
        if (Time.realtimeSinceStartup - lastInputTime < inputCooldown)
            return;

        if (navigationInput.y > 0.5f)
        {
            lastInputTime = Time.realtimeSinceStartup;


        }
        else if (navigationInput.y < -0.5f)
        {

            lastInputTime = Time.realtimeSinceStartup;

        }
    }


 
    public void SelectFirstMenuButton()
    {
        eventSystem.SetSelectedGameObject(firstButton.gameObject);
    }

    public void SelectFirstDeathButton()
    {
        eventSystem.SetSelectedGameObject(firstDeathButton.gameObject);
    }
    

}
