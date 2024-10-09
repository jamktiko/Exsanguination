using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmiliaScripts;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int healValue;
    [SerializeField] GameObject thisPotion;
    //[SerializeField] ParticleSystem healthpotionGlow;
    AudioManager audioManager;
    PlayerHealthManager playerHealthManager;

    private void Awake()
    {
        thisPotion = this.gameObject;
        audioManager = FindObjectOfType<AudioManager>();
        playerHealthManager = FindObjectOfType<PlayerHealthManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player drank a health potion.");
            playerHealthManager.UpdatePlayerHealth(healValue);
            audioManager.PlayPlayerHealAudioClip();
            thisPotion.SetActive(false);
        }
    }
}
