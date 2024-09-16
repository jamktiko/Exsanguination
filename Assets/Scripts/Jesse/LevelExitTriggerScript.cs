using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitTriggerScript : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Player")
        {
            levelManager.CheckExit();
        }
    }
}
