using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFunctions : MonoBehaviour
{
    bool canOpenDoor;
    bool doorIsOpening;
    [SerializeField] Vector3 rotation;
    [SerializeField] float rotationSpeed;
    Transform doorTransform;
    float RotationAmount = 90f;
    Vector3 StartRotation;

    private void Awake()
    {
        doorTransform = GetComponent<Transform>();
        StartRotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (doorIsOpening)
        {
            StartCoroutine(DoorRoation());
        }
    }

    public void OnOpenDoorPressed()
    {
        if (!canOpenDoor) return;

        doorIsOpening = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canOpenDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canOpenDoor = false;
        }
    }

    IEnumerator DoorRoation()
    {
        Quaternion startRotation = doorTransform.rotation;
        Quaternion endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * rotationSpeed;
        }
    }
}
