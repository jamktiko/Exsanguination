using EmiliaScripts;
using UnityEngine;

public class TrapHurt : MonoBehaviour
{

    [SerializeField] int trapDamage;
    [SerializeField] float hurtCooldown;
    PlayerHealthManager playerHealthManager;

    private void Awake()
    {
        playerHealthManager = GameObject.FindWithTag("HealthManager").GetComponent<PlayerHealthManager>();
    }


    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            hurtCooldown = Time.time;
            EnemyHealthScript enemyHealthScript = other.GetComponent<EnemyHealthScript>();
            enemyHealthScript.ChangeEnemyHealth(trapDamage);
        }

        if (other.gameObject.tag == "Player")
        {
            playerHealthManager.UpdatePlayerHealth(-trapDamage);
        }
    }
}