using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;

    
    [SerializeField] HealthBarScript healthBar;

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBarScript>();
        healthBar.UpdateHealthBar(health, maxHealth);
        health = maxHealth;
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
}
