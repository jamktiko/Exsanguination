using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// Assign Layers, make sure that the Player game object has layer Player in correct game object (usually in the one where the movement script is too)
    /// Replace PlayerMovementScript to current player movement script
    /// Enemy uses Rigidbody, adjust its' mass to correspond the creatures weight
    /// Jumpforce is calulated as follows: Rigidbody's mass * 8
    /// Obstacle check distance: if you want the enemy to jump earlier from the wall then increase the value, if you want it to jump closer to the wall then decrease the value
    /// IgnoreLayers: add Layers you want the enemy to avoid when trying to jump over such as Player, otherwise it will try to jump over player
    /// </summary>
    /// 


    [SerializeField] private float moveSpeed = 2f;         // Movement speed of the enemy
    [SerializeField] private float originalSpeed = 2f;         // Movement speed of the enemy
    [SerializeField] private float detectionRange = 10f;   // How close the player has to be for the enemy to detect
    [SerializeField] private float stoppingDistance = 1.5f; // Distance from the player to stop moving
    [SerializeField] private float jumpForce = 5f;         // Force applied when jumping
    [SerializeField] private float pounceForceUp;
    [SerializeField] private float pounceForceForward;
    [SerializeField] private float pounceCooldown = 3.0f; // Time in seconds between attacks
    [SerializeField] private float obstacleCheckDistance = 1.5f; // Distance to check for obstacles
    [SerializeField] private float jumpHeightThreshold = 1.0f;   // How much higher the player has to be for the enemy to jump
    [SerializeField] private LayerMask groundLayer;        // Ground layer for jumping checks
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask ignoreLayers; // LayerMask to specify which layers to ignore when triggering jumping over obstacle
    [SerializeField] private float jumpCooldown = 2f;      // Time in seconds between jumps
    [SerializeField] private PlayerMovement playerMovementScript; // Reference to the player's movement script
    [SerializeField] private float attackRange = 1.5f; // Range within which the enemy will attack the player
    [SerializeField] private float attackCooldown = 1.0f; // Time in seconds between attacks
    [SerializeField] private float pounceRangeMax = 5f;
    [SerializeField] private float pounceRangeMin;
    [SerializeField] private float separationDistance = 2f; // Distance to separate from other enemies
    [SerializeField] private float stopSeparationDistance = 1.5f; // Distance from the player to stop separating

    private Transform player;
    private Rigidbody rb;
    private bool isGrounded;
    private float lastJumpTime = -Mathf.Infinity;  // Stores the time of the last jump
    private float lastAttackTime = -Mathf.Infinity; // Stores the time of the last attack
    private float lastPounceTime = -Mathf.Infinity;
    private float storedSeparationDistance;
    [SerializeField] private Animator enemyAnimator;
    private EnemyHealthScript enemyHealthScript;

    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioSource enemyAlertAudioSource;
    [SerializeField] AudioSource enemyFootstepAudioSource;
    bool hasAlerted;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Check if the Rigidbody component exists
        if (rb == null)
        {
            // Add a Rigidbody component if it doesn't exist
            rb = gameObject.AddComponent<Rigidbody>();
            rb = GetComponent<Rigidbody>();
        }
        enemyHealthScript = GetComponent<EnemyHealthScript>();
    }

    private void Start()
    {
        storedSeparationDistance = separationDistance;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
      
            CheckGroundedStatus();
            AvoidOtherEnemies();
        
        

    }

    private void FixedUpdate()
    {
       
            DetectPlayer();
        
    }


    // Slow the enemy by reducing their movement speed
    public void ApplySlow(float slowAmount)
    {
        moveSpeed = originalSpeed * (1f - slowAmount);
        // Apply movement speed change to AI/movement logic here
    }

    // Remove the slow effect (return to normal speed)
    public void RemoveSlow()
    {
        moveSpeed = originalSpeed;
        // Reset movement speed logic here
    }

    void AvoidOtherEnemies()
    {

        
            // Use a Physics overlap sphere to detect nearby enemies
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, separationDistance, enemyLayer);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform != transform) // Ignore self
                {
                    Vector3 directionToAvoid = transform.position - hitCollider.transform.position;
                    rb.AddForce(directionToAvoid.normalized * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
                }
            }
        

    }

    void DetectPlayer()
    {
        // Use a Physics overlap sphere to detect player in a certain range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        if (hitColliders.Length > 0)
        {
            player = hitColliders[0].transform;  // Assumes there is only one player in the game

            if (!hasAlerted)
            {
                audioManager.PlayEnemyAlertAudioClip(enemyAlertAudioSource);
                hasAlerted = true;
            }

            FollowPlayer();
        }
        else
        {
            hasAlerted = false;
        }
    }

    void FollowPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Calculate the distance to the player
        float distance = Vector3.Distance(transform.position, player.position);

        // Move towards the player if outside stopping distance
        if (distance > stoppingDistance)
        {
            MoveTowardsPlayer(direction);
        }

        // Face the player
        RotateTowardsPlayer(direction);

        // Check if the player is within attack range and attack if possible
        if (distance <= attackRange && CanAttack())
        {
            Attack();
            lastAttackTime = Time.time;

        }
        if (distance <= pounceRangeMax && distance >= pounceRangeMin && CanPounce())
        {
            Pounce();
            lastPounceTime = Time.time;
        }

        if (distance <= stopSeparationDistance)
        {
            separationDistance = 0;
        }

        else
            separationDistance = storedSeparationDistance;



    }
    void Attack()
    {
        Debug.Log("enemy is attacking");
        enemyAnimator.SetBool("isAttacking", true);

    }

    void Pounce()
    {

        Debug.Log("enemy pounced");
        enemyAnimator.SetBool("isAttacking", false);
        enemyAnimator.SetTrigger("pounce");
        Vector3 direction = (player.position - transform.position).normalized;
        rb.AddForce(Vector3.up * pounceForceUp + direction * pounceForceForward, ForceMode.Impulse);

    }

    bool CanPounce()
    {
        return Time.time >= lastPounceTime + pounceCooldown;

    }

    bool CanAttack()
    {
        // Return true if enough time has passed since the last attack
        return Time.time >= lastAttackTime + attackCooldown;
    }


    void MoveTowardsPlayer(Vector3 direction)
    {
        enemyAnimator.SetBool("isAttacking", false);
        // Jump if the player is higher than the enemy, but only if the player is grounded
        if (player.position.y > transform.position.y + jumpHeightThreshold && isGrounded && CanJump() && playerMovementScript.isGrounded)
        {
            Jump();
        }
        else
        {
            // Check for obstacles in front and jump if necessary, only if the player is grounded
            if (IsObstacleInFront() && isGrounded && CanJump() && playerMovementScript.isGrounded)
            {
                Jump();
            }
            else
            {
                // Move the enemy forward
                Vector3 move = new Vector3(direction.x, 0, direction.z);
                rb.MovePosition(transform.position + move * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void RotateTowardsPlayer(Vector3 direction)
    {
        // Calculate the rotation needed to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Jump()
    {
        enemyAnimator.SetTrigger("jump");
        enemyAnimator.SetBool("isAttacking", false);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        lastJumpTime = Time.time;  // Record the time of the jump
    }

    bool IsObstacleInFront()
    {
        // Raycast in front of the enemy to detect obstacles
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, obstacleCheckDistance, ~ignoreLayers))  // Invert LayerMask to ignore specified layers
        {
            return true;
        }
        return false;
    }


    void CheckGroundedStatus()
    {
        // Raycast down to check if the enemy is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    bool CanJump()
    {
        // Return true if enough time has passed since the last jump
        return Time.time >= lastJumpTime + jumpCooldown;
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw obstacle check ray
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstacleCheckDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopSeparationDistance);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, pounceRangeMax);

        // Draw attack range in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // Draw attack range sphere
    }
}
