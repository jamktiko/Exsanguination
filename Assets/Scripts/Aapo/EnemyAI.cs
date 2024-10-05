using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Include NavMesh

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float originalSpeed = 2f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float pounceForceUp;
    [SerializeField] private float pounceForceForward;
    [SerializeField] private float pounceCooldown = 3.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float pounceRangeMax = 5f;
    [SerializeField] private float pounceRangeMin;
    [SerializeField] private float separationDistance = 2f;
    [SerializeField] private float stopSeparationDistance = 1.5f;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioSource enemyAlertAudioSource;
    [SerializeField] private AudioSource enemyFootstepAudioSource;
    
    private NavMeshAgent navMeshAgent;  // NavMeshAgent reference
    private Transform player;
    private Rigidbody rb;
    public bool isGrounded;
    private float lastAttackTime = -Mathf.Infinity;
    private float lastPounceTime = -Mathf.Infinity;
    private float storedSeparationDistance;
    private bool hasAlerted;
    private EnemyStates enemyStates;
    public bool enemyIsTriggered;
    private bool isPouncing;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        navMeshAgent = GetComponent<NavMeshAgent>(); // Initialize NavMeshAgent
        enemyStates = GetComponentInChildren<EnemyStates>();
        if (player == null)
        {
            // Search for the player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private void Start()
    {
        storedSeparationDistance = separationDistance;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        navMeshAgent.speed = moveSpeed;  // Sync NavMeshAgent speed
        navMeshAgent.stoppingDistance = stoppingDistance;
    }



    private void FixedUpdate()
    {
        if (enemyIsTriggered)
        {
            CheckGroundedStatus();
            AvoidOtherEnemies();
            DetectPlayer();
        }

    }


    // Slow the enemy by reducing their movement speed
    public void ApplySlow(float slowAmount)
    {
        moveSpeed = originalSpeed * (1f - slowAmount);
        navMeshAgent.speed = moveSpeed; // Apply slow to NavMeshAgent
    }

    // Remove the slow effect
    public void RemoveSlow()
    {
        moveSpeed = originalSpeed;
        navMeshAgent.speed = moveSpeed; // Reset speed in NavMeshAgent
    }

    void AvoidOtherEnemies()
    {
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

   public void ActivateEnemy()
    {
        enemyIsTriggered = true;
        enemyAnimator.SetTrigger("detect");
        enemyAnimator.ResetTrigger("detect");
    }

    private void DetectPlayer()
    {

        if (player != null && !enemyStates.isStunned)
        {
            FollowPlayer();
            if (!hasAlerted)
            {
                audioManager.PlayEnemyAlertAudioClip(enemyAlertAudioSource);
                hasAlerted = true;
            }
        }
    }

    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 direction = (player.position - transform.position).normalized;
        RotateTowardsPlayer(direction); // Always rotate towards the player

        if (!isPouncing)
        {
            navMeshAgent.SetDestination(player.position);  // Use NavMesh to move towards player
            enemyAnimator.SetBool("isAttacking", false);
        }


        // Check if the player is within attack or pounce range
        if (distance <= attackRange && CanAttack() && !enemyStates.isStunned)
        {
            Attack();
            lastAttackTime = Time.time;
        }
        if (distance <= pounceRangeMax && distance >= pounceRangeMin && CanPounce() && !enemyStates.isStunned)
        {
            Debug.Log("CanPounce");
            Pounce();
            lastPounceTime = Time.time;
        }

        if (distance <= stopSeparationDistance)
        {
            separationDistance = 0;
        }
        else
        {
            separationDistance = storedSeparationDistance;
        }
    }

    

    void Attack()
    {
        Debug.Log("Enemy is attacking");
        enemyAnimator.SetBool("isAttacking", true);
    }

    void Pounce()
    {
        isPouncing = true;
        Debug.Log("Enemy pounced");
        Vector3 direction = (player.position - transform.position).normalized;
        enemyAnimator.SetTrigger("pounce");
        navMeshAgent.enabled = false;
        Vector3 pounceForce = Vector3.up * pounceForceUp + direction * pounceForceForward;
        rb.AddForce(pounceForce, ForceMode.Impulse);
        StartCoroutine(ReEnableNavMeshAfterJump());
    }


    bool CanPounce()
    {
        return Time.time >= lastPounceTime + pounceCooldown;
        
    }

    bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    void RotateTowardsPlayer(Vector3 direction)
    {
        // Calculate the rotation needed to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

   
    IEnumerator ReEnableNavMeshAfterJump()
    {
        yield return new WaitForSeconds(2); // Wait for the jump to complete
        navMeshAgent.enabled = true;  // Re-enable NavMeshAgent
        isPouncing = false;
        navMeshAgent.isStopped = false;
    }
   


    void CheckGroundedStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

  
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopSeparationDistance);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, pounceRangeMax);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
