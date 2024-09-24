using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;

    
    [SerializeField] HealthBarScript healthBar;
    [SerializeField] StakeLogic stakeLogic;

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBarScript>();
        healthBar.UpdateHealthBar(health, maxHealth);
        health = maxHealth;
        stakeLogic = GameObject.FindGameObjectWithTag("Stake").GetComponent<StakeLogic>();
    }
    
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    public void ChangeEnemyHealth(int changeAmount)
    {
        health = Mathf.Clamp(health - changeAmount, 0, maxHealth);
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    // Get the enemy's current health
    public int GetEnemyHealth()
    {
        return health;
    }
    public int GetEnemyMaxHealth()
    {
        return maxHealth;
    }

    public void Finish()
    {
        // Trigger death animation or effects here
        stakeLogic.UnstickFromEnemy();
        Die();
    }

    // Die and destroy the enemy
    private void Die()
    {
        // Handle enemy death logic here
        Destroy(gameObject);
    }
}
