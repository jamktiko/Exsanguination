using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rebindUIPrefab;      // Prefab for each rebind button
    public Transform rebindUIParent;       // Parent UI element to hold the rebind buttons
    public GameObject waitingForInputPanel; // UI Panel to display when waiting for input

    public InputActionAsset inputActions;  // Input Action Asset containing all bindings
    private Dictionary<string, InputBinding> originalBindings; // Original bindings to reset if needed
    private const string BINDINGS_SAVE_KEY = "bindings";  // PlayerPrefs key for saved bindings

    private InputActionRebindingExtensions.RebindingOperation rebindOperation; // The current rebind operation
    private bool isRebinding = false;      // To track if we're currently rebinding

    private void Start()
    {
        originalBindings = new Dictionary<string, InputBinding>();

        // Load previously saved bindings (if any)
        LoadBindings();

        // Generate UI for each binding
        GenerateRebindingUI();
    }

    private void Update()
    {
        // Check for cancellation inputs (ESC key or Options button) when rebinding
        if (isRebinding && (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame))
        {
            CancelRebinding();
        }
    }

    // Method to load all saved bindings
    private void LoadBindings()
    {
        if (PlayerPrefs.HasKey(BINDINGS_SAVE_KEY))
        {
            string savedBindings = PlayerPrefs.GetString(BINDINGS_SAVE_KEY);
            inputActions.LoadFromJson(savedBindings);
        }
    }

    // Method to save all bindings
    private void SaveBindings()
    {
        string bindingsToSave = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(BINDINGS_SAVE_KEY, bindingsToSave);
    }

    // Generates UI buttons for each binding to allow rebinding
    private void GenerateRebindingUI()
    {
        foreach (InputAction action in inputActions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];

                if (binding.isComposite || binding.isPartOfComposite)
                    continue;  // Skip composite bindings for simplicity

                string bindingDisplayName = InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

                // Create a button in the UI for rebinding this action
                GameObject rebindUIObject = Instantiate(rebindUIPrefab, rebindUIParent);
                Button rebindButton = rebindUIObject.GetComponentInChildren<Button>();
                TMP_Text bindingText = rebindUIObject.GetComponent<TMP_Text>();

                // Display the current binding in the button
                bindingText.text = $"{action.name}: {bindingDisplayName}";

                // Store original binding (can use the binding ID to reference later)
                originalBindings[binding.id.ToString()] = binding;

                // Set button callback for rebinding
                int bindingIndex = i;  // Capture index in local scope for delegate
                rebindButton.onClick.AddListener(() => StartRebinding(action, bindingIndex));
            }
        }
    }

    // Starts the rebinding process for the given action and binding index
    private void StartRebinding(InputAction action, int bindingIndex)
    {
        // Show the UI waiting for input
        waitingForInputPanel.SetActive(true);
        isRebinding = true;

        // Perform rebinding with a mask to target the specific binding index
        rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/position") // Exclude mouse movement
            .WithControlsExcluding("<Gamepad>/leftStick") // Exclude left stick movement
            .WithControlsExcluding("<Gamepad>/rightStick") // Exclude right stick movement
            .OnMatchWaitForAnother(0.1f)  // To wait for accidental double input
            .OnComplete(operation => RebindingComplete(action, bindingIndex, operation))
            .Start();
    }

    // Cancels the current rebinding operation
    private void CancelRebinding()
    {
        // Hide the UI waiting for input
        waitingForInputPanel.SetActive(false);
        isRebinding = false;

        // If the rebind operation exists, cancel it
        if (rebindOperation != null)
        {
            rebindOperation.Dispose();
            rebindOperation = null;
        }

        // Optionally, refresh the UI after cancellation
        RefreshUI();
    }

    // Callback for when rebinding is complete
    private void RebindingComplete(InputAction action, int bindingIndex, InputActionRebindingExtensions.RebindingOperation rebindOperation)
    {
        // Hide the UI waiting for input
        waitingForInputPanel.SetActive(false);
        isRebinding = false;

        // Ensure no duplicate bindings by removing old ones
        string newPath = rebindOperation.selectedControl.path;
        RemoveDuplicateBindings(action, newPath, bindingIndex);

        // Save new binding
        rebindOperation.Dispose();
        SaveBindings();

        // Refresh UI to display updated bindings
        RefreshUI();
    }

    // Removes any duplicate bindings that conflict with the newly assigned one
    private void RemoveDuplicateBindings(InputAction action, string newPath, int bindingIndex)
    {
        foreach (var otherAction in inputActions)
        {
            for (int i = 0; i < otherAction.bindings.Count; i++)
            {
                var binding = otherAction.bindings[i];
                if (binding.effectivePath == newPath && i != bindingIndex)
                {
                    otherAction.RemoveBindingOverride(i);
                }
            }
        }

        // Apply new binding to the specified binding index
        action.ApplyBindingOverride(bindingIndex, newPath);
    }

    // Resets all bindings to their original configuration
    public void ResetBindings()
    {
        foreach (var action in inputActions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if (originalBindings.ContainsKey(binding.id.ToString()))
                {
                    action.ApplyBindingOverride(i, originalBindings[binding.id.ToString()].effectivePath);
                }
            }
        }

        // Save and refresh UI
        SaveBindings();
        RefreshUI();
    }

    // Refreshes the UI after rebinding or cancellation
    private void RefreshUI()
    {
        foreach (Transform child in rebindUIParent)
        {
            Destroy(child.gameObject);
        }
        GenerateRebindingUI();
    }
}
