using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerHandler : MonoBehaviour
{
    public bool controllerIsConnected;
    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void ControllerEnabled()
    {
        controllerIsConnected = true;
        Cursor.visible = false;
    }

    private void ControllerDisabled()
    {

        Cursor.visible = true;
        controllerIsConnected = false;
    }


    private void Update()
    {
        if (Gamepad.all.Count > 0)
        {
            ControllerEnabled();
        }
        else
        {

            ControllerDisabled();
        }
    }
    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    public void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                // New Device.
                ControllerEnabled();
                break;
            case InputDeviceChange.Disconnected:
                ControllerDisabled();
                // Device got unplugged.
                break;
            case InputDeviceChange.Reconnected:
                // Plugged back in.
                ControllerEnabled();
                break;
            case InputDeviceChange.Removed:
                ControllerDisabled();
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                break;
            default:
                // See InputDeviceChange reference for other event types.
                break;
        }
    }
}
