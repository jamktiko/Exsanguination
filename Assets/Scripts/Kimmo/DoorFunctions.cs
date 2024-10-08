using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorFunctions : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    bool doorIsOpening;
    bool isOpen;
    bool isRotatingDoor;
    Vector3 rotation;
    [SerializeField] float rotationSpeed;
    Transform doorTransform;
    float RotationAmount = 90f;
    float forwardDirection;

    Vector3 startRotation;
    Vector3 forward;

    private void Awake()
    {
        inputManager = FindAnyObjectByType<InputManager>();
        doorTransform = GetComponent<Transform>();
        startRotation = transform.rotation.eulerAngles;
        forward = transform.right;
    }

    private void Update()
    {
        //if (doorIsOpening)
        //{
        //    /StartCoroutine(DoorRotation());
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && inputManager.openDoor)
        {
            Open(other.transform.position);
        }
    }

    public void Open(Vector3 UserPosition)
    {
        if (!isOpen)
        {
            float dot = Vector3.Dot(forward, (UserPosition - transform.position).normalized);
            Debug.Log($"Dot: {dot.ToString("N3")}");
            StartCoroutine(OpenDoorRotation(dot));
        }
    }

    IEnumerator OpenDoorRotation(float forwardAmount)
    {
        Quaternion StartRotation = doorTransform.rotation;
        Quaternion EndRotation;

        if (forwardAmount >= forwardDirection)
        {
            EndRotation = Quaternion.Euler(new Vector3(0, startRotation.y - RotationAmount, 0));
        }
        else
        {
            EndRotation = Quaternion.Euler(new Vector3(0, startRotation.y + RotationAmount, 0));
        }

        isOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(StartRotation, EndRotation, time);
            yield return null;
            time += Time.deltaTime * rotationSpeed;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            StartCoroutine(CloseDoorRotation()); 
        }
    }

    private IEnumerator CloseDoorRotation()
    {
        Quaternion StartRotation = transform.rotation;
        Quaternion EndRotation = Quaternion.Euler(startRotation);

        isOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(StartRotation, EndRotation, time);
            yield return null;
            time += Time.deltaTime * rotationSpeed;
        }
    }
}
