using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rebindUIPrefab;      // Prefab for each rebind item (RebindText)
    public Transform rebindUIParent;       // Parent UI element to hold the rebind items
    public GameObject waitingForInputPanel; // UI Panel to display when waiting for input

    public InputActionAsset inputActions;  // Input Action Asset containing all bindings
    private Dictionary<string, InputBinding> originalBindings; // Original bindings to reset if needed
    private const string BINDINGS_SAVE_KEY = "bindings";  // PlayerPrefs key for saved bindings

    private InputActionRebindingExtensions.RebindingOperation rebindOperation; // The current rebind operation
    private bool isRebinding = false;      // To track if we're currently rebinding

    private void Start()
    {
        Debug.Log("RebindingSystem Start() called.");
        originalBindings = new Dictionary<string, InputBinding>();

        // Store the original bindings when the game starts
        StoreOriginalBindings();

        // Load previously saved bindings (if any)
        LoadBindings();

        // Generate UI for each binding
        GenerateRebindingUI();
    }

    private void Update()
    {
        // Check for cancellation inputs (ESC key) when rebinding
        if (isRebinding && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CancelRebinding();
        }
    }

    // Method to store the original bindings for later reset
    private void StoreOriginalBindings()
    {
        foreach (InputAction action in inputActions)
        {
            Debug.Log($"Processing action: {action.name}");
            foreach (var binding in action.bindings)
            {
                Debug.Log($"Binding found: {binding.effectivePath}");
                if (!originalBindings.ContainsKey(binding.id.ToString()))
                {
                    originalBindings[binding.id.ToString()] = binding;
                }
            }
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

    // Generates UI for each binding to allow rebinding
    private void GenerateRebindingUI()
    {
        Debug.Log("GenerateRebindingUI() called.");

        // Check if inputActions contains any action maps
        if (inputActions == null || inputActions.actionMaps.Count == 0)
        {
            Debug.LogError("No action maps found in inputActions.");
            return;
        }

        // Iterate through each action map
        foreach (var actionMap in inputActions.actionMaps)
        {
            Debug.Log($"Action Map: {actionMap.name}");

            // Now loop through each action in this action map
            foreach (var action in actionMap.actions)
            {
                Debug.Log($"Generating UI for action: {action.name}");

                // Continue with your UI generation logic...
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];

                    // Skip composite bindings or non-keyboard/mouse bindings
                    if (binding.isComposite || binding.isPartOfComposite || !IsKeyboardOrMouseBinding(binding))
                        continue;

                    // Create a new UI item for the keybinding
                    GameObject rebindUIObject = Instantiate(rebindUIPrefab, rebindUIParent);

                    // Find relevant components in the prefab structure
                    TMP_Text rebindText = rebindUIObject.GetComponent<TMP_Text>();
                    TMP_InputField keybindInputField = rebindUIObject.transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();
                    Button resetButton = rebindUIObject.transform.Find("ResetButton").GetComponent<Button>();
                    TMP_Text resetButtonText = rebindUIObject.transform.Find("ResetButton/Text (TMP)").GetComponent<TMP_Text>();

                    // Set the action name in the RebindText field
                    rebindText.text = action.name;

                    // Set the current keybinding in the InputField (TMP)
                    string bindingDisplayName = InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                    keybindInputField.text = bindingDisplayName;

                    // Set the Reset button's text (e.g., "Reset")
                    resetButtonText.text = "Reset";

                    // Store original binding (can use the binding ID to reference later)
                    originalBindings[binding.id.ToString()] = binding;

                    // Set input field callback for rebinding
                    int bindingIndex = i;  // Capture index in local scope for delegate
                    keybindInputField.onSelect.AddListener(delegate { StartRebinding(action, bindingIndex); });

                    // Set button callback for resetting the binding
                    resetButton.onClick.AddListener(() => ResetSingleBinding(action, bindingIndex));
                }
            }
        }
    }

    // Check if the binding is for Keyboard or Mouse only
    private bool IsKeyboardOrMouseBinding(InputBinding binding)
    {
        return binding.groups.Contains("Keyboard") || binding.groups.Contains("Mouse");
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

    // Resets a specific binding to its original configuration
    private void ResetSingleBinding(InputAction action, int bindingIndex)
    {
        var binding = action.bindings[bindingIndex];
        if (originalBindings.ContainsKey(binding.id.ToString()))
        {
            // Remove override instead of applying the original path directly
            action.RemoveBindingOverride(bindingIndex);
        }

        // Save and refresh UI
        SaveBindings();
        RefreshUI();
    }

    // Resets all bindings to their original configuration
    public void ResetBindings()
    {
        foreach (var action in inputActions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                action.RemoveBindingOverride(i);
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
