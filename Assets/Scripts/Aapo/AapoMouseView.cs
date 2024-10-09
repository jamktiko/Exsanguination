using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AapoMouseView : MonoBehaviour
{
    [SerializeField] private Transform playercam;
    [SerializeField] private Vector2 Sensitivities;
    private Vector2 XYRotation;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        Vector2 MouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        XYRotation.x -= MouseInput.y * Sensitivities.y;
        XYRotation.y += MouseInput.x * Sensitivities.x;

        XYRotation.x = Mathf.Clamp(XYRotation.x, -90f, 90f);

        transform.eulerAngles = new Vector3(0, XYRotation.y, 0);
        playercam.localEulerAngles = new Vector3(XYRotation.x, 0, 0);
    }
}
