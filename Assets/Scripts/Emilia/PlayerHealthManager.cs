using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EmiliaScripts
{
    public class PlayerHealthManager : MonoBehaviour
    {
        [SerializeField] int currentHealth;
        [SerializeField] int maxHealth;

        HealthVFXUpdater healthVFXUpdater;

        public delegate void DeathInvokerEvent();
        /// <summary>
        /// Called when the player is at 0 or less health.
        /// </summary>
        public event DeathInvokerEvent OnDeath;

        public delegate void HealthUpdate();
        /// <summary>
        /// Called when the health gets updated.
        /// </summary>
        public event HealthUpdate OnHealthUpdate;

        private void Awake()
        {
            healthVFXUpdater = GetComponent<HealthVFXUpdater>();
        }

        void Start()
        {
            currentHealth = maxHealth;
            Debug.Log("Updated Player Health to MAX: " + currentHealth);
        }

        /// <summary>
        /// Returns the  current player health
        /// </summary>
        /// <returns>Int</returns>
        public int CurrentPlayerHealth()
        {
            return currentHealth;
        }

        public int MaxPlayerHealth()
        {
            return maxHealth;
        }

        /// <summary>
        /// Sets the player health to an Int value. Should only be used by save manager, if you see this text you are using it wrong!
        /// </summary>
        public void SetPlayerHealth(int health)
        {
            currentHealth = health;
        }


        /// <summary>
        /// Updates the player health. Input a negative number for damage and a positive for healing.
        /// </summary>
        /// <param name="healthNumber">Int</param>
        public void UpdatePlayerHealth(int healthNumber)
        {
            if (healthNumber != 0 && currentHealth > 0 && currentHealth <= maxHealth)
            {
                currentHealth += healthNumber;
                if (currentHealth >= maxHealth) {
                    currentHealth = maxHealth;
                }
                else if (currentHealth <= 0)
                {
                    OnDeath?.Invoke();
                }
                OnHealthUpdate?.Invoke();
                //Debug.Log("Updating player health with modifier: " + healthNumber);
            }
            else if (currentHealth <= 0)
            {
                OnDeath?.Invoke();
                //Debug.Log("Player is dead. Health: " + currentHealth);
            }
            else if (currentHealth > maxHealth) // avoid overheal
            {
                currentHealth = maxHealth;
                //Debug.Log("Current Health over max, setting to max health: " + currentHealth);
            }

            //Debug.Log("Current Player Health: " + currentHealth);
            healthVFXUpdater.UpdateInjuryVFX(currentHealth);
            if (healthNumber < 0) //check for damage
            {
                StopAllCoroutines();
                StartCoroutine(healthVFXUpdater.FlashDamageTakenVFXCoroutine());
            }
            if (healthNumber > 0)
            {
                healthVFXUpdater.HealingVFXActivate();
            }
        }


    }
}