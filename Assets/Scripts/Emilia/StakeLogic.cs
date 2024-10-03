using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StakeLogic : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float throwForce = 50f, stickDuration = 10f, returnCooldown = 10f, slowAmount = 0.5f, finisherThreshold = 50f, retrievalRange = 2f, stickTimer = 0f;
    private bool isThrown = false, isStuck = false, isReturning = false;

    private Rigidbody rb;
    private EnemyAI stuckEnemy;
    private EnemyHealthScript stuckEnemyHealth;
    private PlayerHealthManager playerHealth;
    private Transform playerTransform;
    public Camera playerCamera;

    private Quaternion stakeRotation = Quaternion.Euler(90f, 0f, 0f);

    [SerializeField] private GameObject stakeLocationOnPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        playerHealth = GameObject.FindWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        Physics.IgnoreCollision(playerTransform.GetComponent<Collider>(), gameObject.GetComponent<Collider>()); //avoid pushing player
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isReturning)
        {
            // If close enough to player, stop returning
            if (Vector3.Distance(transform.position, playerTransform.position) < 0.5f)
            {
                isReturning = false;
                isThrown = false;
                rb.isKinematic = true; // Stop physics interaction
                transform.SetParent(playerTransform, true); // Reattach stake to player
            }
        }
    }

    public void ThrowStake(float timer)
    {
        gameObject.SetActive(true);
        if (!isThrown)
        {
            // Detach from player and throw the stake
            transform.SetParent(null);
            isThrown = true;
            isReturning = false;
            rb.isKinematic = false; // Enable physics for throwing
            throwForce = 0f;

            switch (timer)
            {
                case 0f:
                    throwForce = 10f;
                    break;
                case float t when t > 0f && t <= 0.99f:
                    throwForce = Mathf.Lerp(10f, 50f, t);
                    break;
                default:
                    throwForce = 50f; //Max power
                    break;
            }

            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); //Get middle of screen
            rb.AddForce(ray.direction * throwForce, ForceMode.Impulse);

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
            StickToEnemy(collision.gameObject.GetComponent<EnemyAI>());
        }
    }

    private void StickToEnemy(EnemyAI enemy)
    {
        isStuck = true;
        isThrown = false;
        stuckEnemy = enemy;
        rb.isKinematic = true; // Stop physics movement when stuck
        GameObject go = enemy.gameObject.GetComponentInChildren(typeof(StakeSpot)).gameObject;
        transform.SetParent(go.transform);
        Physics.IgnoreCollision(enemy.GetComponent<Collider>(), gameObject.GetComponent<Collider>()); // avoid pushing enemy

        // Apply damage and slowing effect
        //stuckEnemyHealth.ChangeEnemyHealth(stuckEnemyHealth.GetEnemyMaxHealth() / 2);
        enemy.ApplySlow(slowAmount);
    }

    public void UnstickFromEnemy()
    {
        transform.SetParent(playerTransform, true);
        transform.SetPositionAndRotation(stakeLocationOnPlayer.transform.position, stakeRotation);
        playerHealth.UpdatePlayerHealth(playerHealth.MaxPlayerHealth() / 2);
    }

    // Instantly return the stake to the player after the cooldown
    public void ReturnToPlayer()
    {
        if (!isStuck && !isReturning)
        {
            isReturning = true;

            // Stop all physics interactions immediately
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Instantly teleport the stake back to the player's hand
            transform.SetPositionAndRotation(stakeLocationOnPlayer.transform.position, stakeRotation);
            transform.SetParent(playerTransform, true);

            // Reset state
            isThrown = false;
            isReturning = false;
            gameObject.SetActive(false);
        }
    }

    public void RetrieveStake()
    {
        if (isStuck && stuckEnemy != null)
        {
            // Check if within retrieval range
            if (Vector3.Distance(playerTransform.position, transform.position) <= retrievalRange)
            {
                // If health < 50%, apply finisher
                //if (stuckEnemyHealth.GetEnemyHealth() <= stuckEnemyHealth.GetEnemyMaxHealth() / 2)
                //{
                //    stuckEnemyHealth.Finish();
                //}
                //else
                //{
                //    stuckEnemy.RemoveSlow(); // Just remove the slow effect
                //}

                // Unstick the stake
                ReturnToPlayer();
                stuckEnemy = null;
                isStuck = false;
                
            }
        }
    }

}
