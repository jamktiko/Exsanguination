using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private int openSpeed;

    [SerializeField] private GameObject leftOrigin;
    [SerializeField] private GameObject rightOrigin;

    [SerializeField] private Vector3 openRotation;
    [SerializeField] private Vector3 closedRotation;
    void Start()
    {
        closedRotation = transform.rotation.eulerAngles;
    }


    void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            OpenDoor();
        }
    }



    public void OpenDoor()
    {
        Vector3 rotationDirection = closedRotation + openRotation;

        rightOrigin.transform.Rotate(rotationDirection * Time.deltaTime * openSpeed);
    }
}
