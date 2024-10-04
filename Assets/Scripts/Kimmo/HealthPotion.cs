using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmiliaScripts;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int healValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player drank a health potion.");
            PlayerHealthManager playerHealthManager;
            playerHealthManager = other.GetComponent<PlayerHealthManager>();
            playerHealthManager.UpdatePlayerHealth(healValue);
        }
    }
}
