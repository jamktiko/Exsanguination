using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatProjectile : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject particleEffect;
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
        if(collision.collider.CompareTag("Player"))
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

        else if (isReflected && collision.collider.CompareTag("Enemy"))
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
        // Store the target position as the player's position when this object is enabled
        Vector3 targetPosition = player.position;

        // Detach the particle effect to prevent rotation inheritance
        Transform originalParent = particleEffect.transform.parent;
        particleEffect.transform.parent = null;

        // Reset any existing velocity to avoid inherited movement
        rb.velocity = Vector3.zero;

        // Calculate the direction to the player’s initial position
        Vector3 directionToPlayer = (targetPosition - particleEffect.transform.position).normalized;

        // Set the rotation explicitly based on the target direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        particleEffect.transform.rotation = targetRotation;

        // Reattach the particle effect to its original parent
        particleEffect.transform.parent = originalParent;

        // Start the particle system
        particleEffect.GetComponent<ParticleSystem>().Play();

        // Start the coroutine to apply velocity after a delay
        StartCoroutine(TimeBullet(directionToPlayer, originalParent));
    }

    IEnumerator TimeBullet(Vector3 directionToPlayer, Transform originalParent)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        rb.velocity = directionToPlayer * projectileSpeed;

        yield return new WaitForSecondsRealtime(5f);

        particleEffect.transform.parent = originalParent;
    }
}
