using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentActivation : MonoBehaviour
{
    Transform gasObject;
    Collider gasCollider;

    private void Awake()
    {
        gasObject = transform.Find("Gas");
        gasCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        gasObject.gameObject.SetActive(false);
        gasCollider.enabled = false;
    }

    public void SetGasActive()
    {
        gasObject.gameObject.SetActive(true);
        gasCollider.enabled = true;
    }
}
