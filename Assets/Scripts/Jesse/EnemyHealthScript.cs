using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;
    [SerializeField] float finisherTime = 1f; //seconds how long does the finisher take until enemy dies
    
    [SerializeField] HealthBarScript healthBar;
    [SerializeField] StakeLogic stakeLogic;
    private EnemyRagdoll enemyRagdoll;
    [SerializeField] Animator playerAnimator;
    public bool isBeingFinished;
    [SerializeField] RBInputManager rbInputManager;
    void Awake()
    {
        enemyRagdoll = GetComponent<EnemyRagdoll>();
        healthBar = GetComponentInChildren<HealthBarScript>();
        stakeLogic = GameObject.FindGameObjectWithTag("Stake").GetComponent<StakeLogic>();
    }

    private void Start()
    {
        healthBar.UpdateHealthBar(health, maxHealth);
        health = maxHealth;
    }

   
    
    public void ChangeEnemyHealth(int changeAmount)
    {
        health = Mathf.Clamp(health - changeAmount, 0, maxHealth);
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();        
        }
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
        //stakeLogic.UnstickFromEnemy();
        Debug.Log("finisher started");
        isBeingFinished = true;
        playerAnimator.SetTrigger("finisher");
        enemyRagdoll.ActivateRagdoll();
        //player visual effects
        StartCoroutine(FinisherTimer());
    }

    private IEnumerator FinisherTimer()
    {
        rbInputManager.ControlsEnabled(false);
        yield return new WaitForSeconds(finisherTime);
        rbInputManager.ControlsEnabled(true);
        Die();
    }

    // Die and destroy the enemy
    private void Die()
    {
        // Handle enemy death logic here
        // Destroy(gameObject);
        Debug.Log("enemy died");
    }
}
