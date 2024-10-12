using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ControllerMenuNavigation : MonoBehaviour
{
    public GameObject[] menuButtons;

    public PauseScript pauseScript; // Reference to your pause system
    public EventSystem eventSystem; // Unity EventSystem for handling selections
    private Selectable currentButton; // Current selected button
    public Selectable firstButton; // The default button when the menu is opened
    private bool canNavigate = true; // Flag to ensure navigation happens one at a time
    private float navigationCooldown = 0.3f; // Cooldown duration to prevent multiple selections
    public float inputCooldown = 0.2f;  // Cooldown duration in seconds
    private int currentIndex = 0;
    private float lastInputTime = 0f;

    private void Start()
    {

        // Set the first selected button when the menu is opened
        if (firstButton != null)
        {
            eventSystem.SetSelectedGameObject(firstButton.gameObject);
            currentButton = firstButton;
        }
    }



    public void Navigate(Vector2 navigationInput)
    {
        if (Time.realtimeSinceStartup - lastInputTime < inputCooldown)
            return;

        if (navigationInput.y > 0.5f)
        {
            currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;
            Debug.Log("Navigated Up to Index: " + currentIndex);
            lastInputTime = Time.realtimeSinceStartup;
        }
        else if (navigationInput.y < -0.5f)
        {
            currentIndex = (currentIndex + 1) % menuButtons.Length;
            Debug.Log("Navigated Down to Index: " + currentIndex);
            lastInputTime = Time.realtimeSinceStartup;
        }

    }
}
