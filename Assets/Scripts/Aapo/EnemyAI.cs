using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private EnemyStates enemyStates;
    public bool enemyIsTriggered;
    private bool isPouncing;
    private Vector3 pounceDirection;
    private bool canMoveAfterPounce;
    private bool isStuckOnStake;
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
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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

            if (isPouncing)
            {
                navMeshAgent.enabled = false;
                rb.AddForce(pounceDirection * pounceForceForward, ForceMode.Impulse);
                Debug.Log("Enemy pounced with force " + pounceForceUp + " up and " + pounceDirection + " forward");
                isPouncing = false;
            }


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
        audioManager.PlayEnemyAlertAudioClip(enemyAlertAudioSource);

    }

    private void DetectPlayer()
    {
        
        if (player != null && !enemyStates.isStunned)
        {
            FollowPlayer();
            
        }
    }

    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 direction = (player.position - transform.position).normalized;

        if(!enemyStates.isStunned && !enemyAnimator.GetBool("isAttacking") && distance > attackRange && navMeshAgent.enabled && isGrounded)
        {
            navMeshAgent.updateRotation = true;
            //RotateTowardsPlayer(direction); // Always rotate towards the player
            navMeshAgent.SetDestination(player.position);  // Use NavMesh to move towards player
        }



        // Check if the player is within attack or pounce range
        if (distance <= attackRange)
        {
            if (CanAttack() && !enemyStates.isStunned)
            {
                navMeshAgent.updateRotation = false; // Let NavMeshAgent handle rotation
                SnapRotationTowardsPlayer(direction);
                Attack();  // Trigger the attack
            }
        }


        if (distance <= pounceRangeMax && distance >= pounceRangeMin && CanPounce() && !enemyStates.isStunned && !enemyAnimator.GetBool("isAttacking") && !isStuckOnStake)
        {
            navMeshAgent.updateRotation = false; // Let NavMeshAgent handle rotation
            SnapRotationTowardsPlayer(direction);
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
        // Check if the enemy is already attacking
        if (enemyAnimator.GetBool("isAttacking"))
            return;  // If attacking, do nothing (don't reset or start another attack)
        Debug.Log("Enemy is attacking");
        lastAttackTime = Time.time;  // Record the last time an attack was initiated
        enemyAnimator.SetBool("isAttacking", true);  // Start attack animation
        StartCoroutine(ResetAttackAfterCooldown());  // Reset attack state after the animation
    }
    IEnumerator ResetAttackAfterCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);  // Wait for attackCooldown duration
        enemyAnimator.SetBool("isAttacking", false);  // Reset attacking state
        navMeshAgent.updateRotation = true;
    }

    void Pounce()
    {
        pounceDirection = (player.position - transform.position).normalized;
        pounceDirection.y = pounceForceUp;
        enemyAnimator.SetTrigger("pounce");
        isPouncing = true;
        StartCoroutine(PounceWait());
    }

    IEnumerator PounceWait()
    {
        if (!isGrounded)
        {
            rb.drag = 0;
        }
        //after 2 seconds of pounce if enemy is on ground turn ai movement logic back on
        yield return new WaitForSeconds(2);
       
        if (isGrounded)
        {
            
            navMeshAgent.enabled = true;
            rb.drag = 2;
        }
        else
        {
            //if the enemy is still in the air, start new timer
            StartCoroutine(ForceNavMesh());  
        }

    }

    IEnumerator ForceNavMesh()
    {
        //force enemy on surface after 5 seconds if its floating in air
        yield return new WaitForSeconds(5);
        navMeshAgent.enabled = true;
        rb.drag = 2;
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
