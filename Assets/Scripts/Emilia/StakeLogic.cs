using EmiliaScripts;
using UnityEngine;

public class StakeLogic : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float throwForce = 50f, stickDuration = 10f, returnCooldown = 10f, slowAmount = 0.5f, finisherThreshold = 0.25f, retrievalRange = 2f, stickTimer = 0f;
    [SerializeField] private bool isThrown = false, isStuck = false, isReturning = false;
    public bool startedFinishing;
    private Rigidbody rb;
    [SerializeField] private EnemyAI stuckEnemy;
    private EnemyHealthScript stuckEnemyHealth;
    private EnemyFinisher stuckEnemyFinisher;
    private Transform playerTransform;
    public Camera playerCamera;

    private Quaternion stakeRotation = Quaternion.Euler(0f, 0f, 0f);

    [SerializeField] private GameObject stakeLocationOnPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        stuckEnemyFinisher = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<EnemyFinisher>();
    }

    void Start()
    {
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
                CompleteReturnToPlayer();
            }

        }
    }

    public void ThrowStake(float timer)
    {
        gameObject.SetActive(true);
        if (!isThrown && !stuckEnemy && !isReturning)
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
            stuckEnemyFinisher.SetEnemyType(stuckEnemy.gameObject.name);
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
        transform.localPosition = Vector3.zero;
        Physics.IgnoreCollision(enemy.GetComponent<Collider>(), gameObject.GetComponent<Collider>()); // avoid pushing enemy

        // Apply damage and slowing effect
        enemy.ApplySlow(slowAmount);
    }

    public void UnstickFromEnemy(bool isFinished)
    {
        // Reset stake to player's position
        transform.SetParent(playerTransform, true);
        transform.position = stakeLocationOnPlayer.transform.position;
        transform.localRotation = stakeRotation;
        startedFinishing = false;
        if (isFinished)
        {
            //playerHealth.UpdatePlayerHealth(playerHealth.MaxPlayerHealth() / 2); I put this into enemyFinisher script when enemy explodes - Aapo
        }

        // Re-enable collision between stake and enemy
        if (stuckEnemy != null)
        {
            Physics.IgnoreCollision(stuckEnemy.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), false);
        }

        // Clear stuck state and enemy references
        isStuck = false;
        stuckEnemy = null;
        stuckEnemyHealth = null;
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

            CompleteReturnToPlayer();
        }
    }

    private void CompleteReturnToPlayer()
    {
        // Instantly teleport the stake back to the player's hand
        transform.SetParent(playerTransform, true);
        transform.position = stakeLocationOnPlayer.transform.position;
        transform.localRotation = stakeRotation;

        // Reset state
        isThrown = false;
        isReturning = false;

        // Reset references for future throws
        isStuck = false;
        stuckEnemy = null;
        stuckEnemyHealth = null;

        gameObject.SetActive(false);
    }

    public void RetrieveStake()
    {
        if (isStuck && stuckEnemy != null)
        {
            // Check if within retrieval range
            if (Vector3.Distance(playerTransform.position, transform.position) <= retrievalRange)
            {
                if (stuckEnemyHealth.GetEnemyHealth() <= (int)(stuckEnemyHealth.GetEnemyMaxHealth() * finisherThreshold))
                {
                    startedFinishing = true;
                    stuckEnemyHealth.FinishEnemy();
                    UnstickFromEnemy(true);
                }
                else
                {
                    stuckEnemy.RemoveSlow();
                    UnstickFromEnemy(false);
                }

                // Cancel previous invoke in case it's still active
                CancelInvoke(nameof(ReturnToPlayer));

                // Ensure stake returns to player
                ReturnToPlayer();
            }
        }
    }

    public void StartThrowingChargingVisual()
    {
        //Charge backwards animation
    }

    public void StartThrowVisual()
    {
        // Throw Stake
    }

}
