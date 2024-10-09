using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmiliaScripts;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int healValue;
    [SerializeField] GameObject thisPotion;
    //[SerializeField] ParticleSystem healthpotionGlow;

    private void Awake()
    {
        thisPotion = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player drank a health potion.");
            PlayerHealthManager playerHealthManager;
            playerHealthManager = other.GetComponent<PlayerHealthManager>();
            playerHealthManager.UpdatePlayerHealth(healValue);
            thisPotion.SetActive(false);
        }
    }
}
