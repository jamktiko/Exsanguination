using UnityEngine.InputSystem;
using UnityEngine;

public class ControllerHandler : MonoBehaviour
{
    public bool controllerIsConnected;
    private bool cursorVisible = true; // Track cursor visibility state.

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    public void ControllerEnabled()
    {
        if (cursorVisible)
        {
            Cursor.visible = false;
            cursorVisible = false;
        }
        controllerIsConnected = true;
    }

    public void ControllerDisabled()
    {
        if (!cursorVisible)
        {
            Cursor.visible = true;
            cursorVisible = true;
        }
        controllerIsConnected = false;
    }

    private void Start()
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
                ControllerEnabled();
                break;
            case InputDeviceChange.Disconnected:
                ControllerDisabled();
                break;
            case InputDeviceChange.Reconnected:
                ControllerEnabled();
                break;
            case InputDeviceChange.Removed:
                ControllerDisabled();
                break;
        }
    }
}
