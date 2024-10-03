using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;
    [SerializeField] AudioManager audioManager;



    private void Start()
    {
        health = maxHealth;
    }

    public void ChangeEnemyHealth(int changeAmount)
    {
        audioManager.PlayEnemyTakeDamageAudioClip();
        health = Mathf.Clamp(health - changeAmount, 0, maxHealth);
        Debug.Log("Enemy health = " + health);
        if (health <= 0)
        {
            Debug.Log("Enemy health is zero");
            Die();
        }
    }

    public int GetEnemyHealth()
    {
        return health;
    }
    public int GetEnemyMaxHealth()
    {
        return maxHealth;
    }

    private void Die()
    {
        audioManager.PlayEnemyDieAudioClip();
        Debug.Log("enemy died");
    }
}
