using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StakeLogic : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public float throwForce = 30f;
    public float stickDuration = 10f;
    public int damageAmount = 20;
    public float slowAmount = 0.5f;
    public float finisherThreshold = 50f;
    public float retrievalRange = 2f;

    private Rigidbody rb;
    private bool isThrown = false;
    private bool isStuck = false;
    private bool isReturning = false;
    private AapoEnemyAI stuckEnemy = null;
    private float stickTimer = 0f;
    private Transform player;
    public Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (isStuck && stuckEnemy != null)
        {
            stickTimer += Time.deltaTime;
            if (stickTimer >= stickDuration)
            {
                // Return the stake to the player after 10 seconds
                ReturnToPlayer();
            }
        }

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
            }
        }
    }

    public void ThrowStake()
    {
        if (!isThrown)
        {
            transform.SetParent(null);
            isThrown = true;
            isReturning = false;
            rb.isKinematic = false; // Enable physics for throwing
            rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse); // Throw in the looking direction
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Stick to the enemy
            StickToEnemy(collision.gameObject.GetComponent<AapoEnemyAI>());
        }
        else
        {
            Invoke(nameof(ReturnToPlayer), stickDuration);
        }
    }

    private void StickToEnemy(AapoEnemyAI enemy)
    {
        isStuck = true;
        isThrown = false;
        stuckEnemy = enemy;
        rb.isKinematic = true; // Stop physics movement when stuck
        transform.SetParent(enemy.transform);

        // Apply damage and slowing effect
        enemy.TakeDamage(damageAmount);
        enemy.ApplySlow(slowAmount);

        // Set timer to remove the stake after stickDuration
        stickTimer = 0f;
    }

    public void UnstickFromEnemy()
    {
        transform.SetParent(player.transform);
        transform.position = player.transform.position;
    }

    // Return the stake to the player after a delay if it's not stuck to an enemy
    private void ReturnToPlayer()
    {
        if (!isStuck && !isReturning)
        {
            isReturning = true;
            StartCoroutine(ReturningToPlayerCoroutine());
        }
    }

    private IEnumerator ReturningToPlayerCoroutine()
    {
        float time = 0f;
        Vector3 startPosition = transform.position;
        while (time < 1f)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, player.transform.position, time);
            yield return null;
        }

        // Reset stake once it reaches player
        isReturning = false;
        transform.SetParent(player.transform);
        transform.position = player.transform.position;
    }

    public void RetrieveStake()
    {
        if (isStuck && stuckEnemy != null)
        {
            // Check if within retrieval range
            if (Vector3.Distance(player.position, transform.position) <= retrievalRange)
            {
                // If health < 50%, apply finisher
                if (stuckEnemy.GetHealth() < finisherThreshold)
                {
                    stuckEnemy.Finish();
                }
                else
                {
                    stuckEnemy.RemoveSlow(); // Just remove slow effect
                }

                // Unstick the stake
                stuckEnemy = null;
                isStuck = false;
                ReturnToPlayer();
            }
        }
    }

}
