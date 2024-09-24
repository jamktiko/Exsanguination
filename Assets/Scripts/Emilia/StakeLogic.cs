using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StakeLogic : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public float throwForce = 50f;
    public float stickDuration = 10f;
    public float returnCooldown = 10f;
    public int damageAmount = 20;
    public float slowAmount = 0.5f;
    public float finisherThreshold = 50f;
    public float retrievalRange = 2f;

    private Rigidbody rb;
    private bool isThrown = false;
    private bool isStuck = false;
    private bool isReturning = false;
    private AapoEnemyAI stuckEnemy = null;
    private EnemyHealthScript stuckEnemyHealth = null;
    private float stickTimer = 0f;
    private Transform player;
    public Camera playerCamera;

    [SerializeField] private GameObject stakeLocationOnPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        Physics.IgnoreCollision(player.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isReturning)
        {
            // Move stake back to player
            Vector3 direction = (player.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * throwForce * Time.deltaTime);

            // If close enough to player, stop returning
            if (Vector3.Distance(transform.position, player.position) < 0.5f)
            {
                isReturning = false;
                isThrown = false;
                rb.isKinematic = true; // Stop physics interaction
                transform.SetParent(player); // Reattach stake to player
            }
        }
    }

    public void ThrowStake()
    {
        gameObject.SetActive(true);
        if (!isThrown)
        {
            // Detach from player and throw the stake
            transform.SetParent(null);
            isThrown = true;
            isReturning = false;
            rb.isKinematic = false; // Enable physics for throwing
            rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse); // Throw in the direction the player is looking

            // Start the countdown immediately after throwing
            Invoke(nameof(ReturnToPlayer), returnCooldown);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Stick to the enemy
            stuckEnemyHealth = collision.gameObject.GetComponent<EnemyHealthScript>();
            StickToEnemy(collision.gameObject.GetComponent<AapoEnemyAI>());
        }
    }

    private void StickToEnemy(AapoEnemyAI enemy)
    {
        isStuck = true;
        isThrown = false;
        stuckEnemy = enemy;
        rb.isKinematic = true; // Stop physics movement when stuck
        GameObject go = enemy.gameObject.GetComponentInChildren(typeof(StakeSpot)).gameObject;
        transform.SetParent(go.transform);
        Physics.IgnoreCollision(enemy.GetComponent<Collider>(), gameObject.GetComponent<Collider>());

        // Apply damage and slowing effect
        stuckEnemyHealth.ChangeEnemyHealth(stuckEnemyHealth.GetEnemyMaxHealth() / 2);
        enemy.ApplySlow(slowAmount);
    }

    public void UnstickFromEnemy()
    {
        transform.SetParent(player);
        transform.position = stakeLocationOnPlayer.transform.position;
        transform.rotation = Quaternion.identity;
    }

    // Instantly return the stake to the player after the cooldown
    private void ReturnToPlayer()
    {
        if (!isStuck && !isReturning)
        {
            isReturning = true;

            // Stop all physics interactions immediately
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Instantly teleport the stake back to the player's hand
            transform.position = stakeLocationOnPlayer.transform.position;
            transform.rotation = Quaternion.identity;
            transform.SetParent(player);

            // Reset state
            isThrown = false;
            isReturning = false;
        }
    }

    public void RetrieveStake()
    {
        if (isStuck && stuckEnemy != null)
        {
            // Check if within retrieval range
            if (Vector3.Distance(player.position, transform.position) <= retrievalRange)
            {
                // If health < 50%, apply finisher
                if (stuckEnemyHealth.GetEnemyHealth() <= stuckEnemyHealth.GetEnemyMaxHealth() / 2)
                {
                    stuckEnemyHealth.Finish();
                }
                else
                {
                    stuckEnemy.RemoveSlow(); // Just remove the slow effect
                }

                // Unstick the stake
                stuckEnemy = null;
                isStuck = false;
                ReturnToPlayer();
            }
        }
    }

}
