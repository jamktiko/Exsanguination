using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatProjectile : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float projectileSpeed;
    private PlayerCombat playerCombat;
    private PlayerHealthManager playerHealthManager;
    private Transform player;             // Reference to the player's position
    private Rigidbody rb;
    public bool isReflected;

    private void Awake()
    {
       playerCombat = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<PlayerCombat>();
       playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        ResetValues();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            if (playerCombat.isBlocking)
            {
                rb.isKinematic = true;
                Vector3 directionToEnemy = (transform.position - player.position).normalized;
                rb.isKinematic = false;
                rb.velocity = directionToEnemy * projectileSpeed;  // Set the velocity directly
                isReflected = true;
                Debug.Log("reflected");
            }
            else
            {
                playerHealthManager.UpdatePlayerHealth(-damage);
                ResetValues();
                Debug.Log("Hit player");
            }

        }

        else if (isReflected && collision.collider.tag == "Enemy")
        {
            EnemyHealthScript enemyHealthScript = collision.collider.GetComponent<EnemyHealthScript>();

            if (enemyHealthScript != null)
            {
                enemyHealthScript.ChangeEnemyHealth(damage);
                Debug.Log("Reflected back to enemy and dealt damage");
            }
            else
            {
                Debug.LogWarning("EnemyHealthScript not found on " + collision.collider.name);
            }
        }

        else

        {
            ResetValues();
            Debug.Log("hit on something else than player");
        }
    }

    private void ResetValues()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        gameObject.SetActive(false);
        isReflected = false;

    }

    private void OnEnable()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        rb.velocity = directionToPlayer * projectileSpeed;  // Set the velocity directly
    }
}
