using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float pounceRangeMax = 5f;
    [SerializeField] private float pounceRangeMin;
    [SerializeField] private float separationDistance = 2f;
    [SerializeField] private float stopSeparationDistance = 1.5f;
    [SerializeField] private Animator enemyAnimator;
    private AudioManager audioManager;
    [SerializeField] private AudioSource enemyAlertAudioSource;
    [SerializeField] private AudioSource enemyFootstepAudioSource;

    public bool pouncingEnemy;
    private NavMeshAgent navMeshAgent;  // NavMeshAgent reference
    private Transform player;
    private Rigidbody rb;
    public bool isGrounded;
    private float lastAttackTime = -Mathf.Infinity;
    private float lastPounceTime = -Mathf.Infinity;
    private float storedSeparationDistance;
    private EnemyStates enemyStates;
    public bool enemyIsTriggered;
    private bool isPouncing;
    private Vector3 pounceDirection;
    private bool canMoveAfterPounce;
    private bool isStuckOnStake;
    private float nextSeparationCheckTime = 0f;  // Timer for frequency control
private Vector3 separationForce;             // Reusable vector for separation force
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>(); // Initialize NavMeshAgent
        enemyStates = GetComponentInChildren<EnemyStates>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
      player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();    
    }

    private void Start()
    {
        storedSeparationDistance = separationDistance;
        navMeshAgent.speed = moveSpeed;  // Sync NavMeshAgent speed
        navMeshAgent.stoppingDistance = stoppingDistance;

        int randomNumber = Random.Range(1, 3);
        if (randomNumber == 1)
        {
            pouncingEnemy = true;
        }

    }



    private void FixedUpdate()
    {
        if (!enemyIsTriggered) return;

        CheckGroundedStatus();
        if (!isPouncing)
        {
            AvoidOtherEnemies();
            DetectPlayer();
            if (isGrounded)
            {
                navMeshAgent.enabled = true;
            }
        }
        else
        {
            navMeshAgent.enabled = false;
            rb.AddForce(pounceDirection * pounceForceForward, ForceMode.Impulse);
            isPouncing = false;
        }
    }


    // Slow the enemy by reducing their movement speed
    public void ApplySlow(float slowAmount)
    {
        moveSpeed = originalSpeed * (1f - slowAmount);
        navMeshAgent.speed = moveSpeed; // Apply slow to NavMeshAgent
        isStuckOnStake = true;
    }

    // Remove the slow effect
    public void RemoveSlow()
    {
        moveSpeed = originalSpeed;
        navMeshAgent.speed = moveSpeed; // Reset speed in NavMeshAgent
        isStuckOnStake = false;
    }








    void AvoidOtherEnemies()
    {
        // Only run this check every 0.2 seconds to reduce overhead
        if (Time.time < nextSeparationCheckTime) return;
        nextSeparationCheckTime = Time.time + 0.2f;

        // Reset the accumulated separation force
        separationForce = Vector3.zero;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, separationDistance, enemyLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform != transform) // Ignore self
            {
                // Accumulate direction to avoid other enemies
                separationForce += (transform.position - hitCollider.transform.position).normalized;
            }
        }

        // Apply the accumulated separation force scaled by moveSpeed and delta time
        rb.AddForce(separationForce * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    public void ActivateEnemy()
    {
        enemyIsTriggered = true;
        enemyAnimator.SetTrigger("detect");
        audioManager.PlayEnemyAlertAudioClip(enemyAlertAudioSource);

    }

    private void DetectPlayer()
    {
        
        if (player != null && !enemyStates.isStunned && !isPouncing)
        {
            FollowPlayer();
            
        }
    }

    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 direction = (player.position - transform.position).normalized;

        if(!enemyStates.isStunned && !enemyAnimator.GetBool("isAttacking") && distance > attackRange && navMeshAgent.enabled && isGrounded && !isPouncing)
        {
            RotateTowardsPlayer(direction); // Always rotate towards the player
            navMeshAgent.SetDestination(player.position);  // Use NavMesh to move towards player
        }



        // Check if the player is within attack or pounce range
        if (distance <= attackRange)
        {
            if (CanAttack() && !enemyStates.isStunned)
            {
                SnapRotationTowardsPlayer(direction);
                Attack();  // Trigger the attack
            }
        }


        if (pouncingEnemy && distance <= pounceRangeMax && distance >= pounceRangeMin && CanPounce() && !enemyStates.isStunned && navMeshAgent.enabled && !enemyAnimator.GetBool("isAttacking") && !isStuckOnStake)
        {
           
            
            {
                SnapRotationTowardsPlayer(direction);
                if (isGrounded)
                {
                    Pounce();
                    lastPounceTime = Time.time;
                }
            }
            
            
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
        // Check if the enemy is already attacking
        if (enemyAnimator.GetBool("isAttacking"))
            return;  // If attacking, do nothing (don't reset or start another attack)
        lastAttackTime = Time.time;  // Record the last time an attack was initiated
        enemyAnimator.SetBool("isAttacking", true);  // Start attack animation
        StartCoroutine(ResetAttackAfterCooldown());  // Reset attack state after the animation
    }
    IEnumerator ResetAttackAfterCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);  // Wait for attackCooldown duration
        enemyAnimator.SetBool("isAttacking", false);  // Reset attacking state
    }

    void Pounce()
    {
        pounceDirection = (player.position - transform.position).normalized;
        pounceDirection.y = pounceForceUp;
        enemyAnimator.SetTrigger("pounce");
        isPouncing = true;
    }

   

    bool CanPounce()
    {
        return Time.time >= lastPounceTime + pounceCooldown;
        
    }

    bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown && !enemyAnimator.GetBool("isAttacking");
    }

    void RotateTowardsPlayer(Vector3 direction)
    {
        // Calculate the rotation needed to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
    }

    void SnapRotationTowardsPlayer(Vector3 direction)
    {
        // Calculate the rotation needed to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Instant rotation
        transform.rotation = lookRotation; // Snap to the player's direction
    }



    void CheckGroundedStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);
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
