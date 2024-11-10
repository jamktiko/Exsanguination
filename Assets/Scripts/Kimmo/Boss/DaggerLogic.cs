using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerLogic : MonoBehaviour
{
    [SerializeField] float daggerSpeed;
    GameObject daggerStartingPoint;
    [SerializeField] GameObject daggerModel;
    public bool isThrown;

    private void Awake()
    {
        daggerStartingPoint = GameObject.Find("DaggerStartingPoint");
        daggerModel.SetActive(false);
    }

    void Update()
    {
        if (isThrown)
        {
            daggerModel.SetActive(true);
            transform.SetParent(null);
            transform.Translate(Vector3.forward * daggerSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.layer == 10)
        {
            isThrown = false;
            transform.SetParent(daggerStartingPoint.transform);
            transform.position = daggerStartingPoint.transform.position;
            daggerModel.SetActive(false);
        }
    }
}
