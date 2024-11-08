using UnityEngine;
using UnityEngine.AI;

public class BatEnemyAI : MonoBehaviour
{
    private Transform player;             // Reference to the player's position
    public float attackRange = 10f;      // The range within which the bat will attack
    public GameObject projectile;  // The projectile prefab to be instantiated
    public float attackCooldown = 1.6f;    // Time between attacks

    private NavMeshAgent agent;          // Reference to the NavMeshAgent component
    private bool isInAttackRange;
    private bool isAttacking = false;    // Indicates if the bat is currently attacking
    private float lastAttackTime;
    private bool enemyIsTriggered;
    private Animator enemyAnimator;
    private AudioManager audioManager;
    private Animator animator;
    private AudioSource audioSource;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); // Initialize NavMeshAgent
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyAnimator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
 
    public void ActivateEnemy()
    {
        enemyIsTriggered = true;
        enemyAnimator.SetTrigger("detect");
        audioManager.PlayEnemyBatAlertClip(audioSource);

    }

    void Update()
    {
        if (enemyIsTriggered)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Look at the player
            LookAtPlayer();



            // If not attacking, decide whether to chase or attack the player
            if (!isAttacking)
            {
                if (distanceToPlayer <= attackRange)
                {
                    // Stop moving and attack the player
                    AttackPlayer();
                }
                else
                {
                    // Chase the player if out of attack range
                    ChasePlayer();
                }
            }
        }
       
    }
    
    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // Keep the bat upright
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth look
    }

    private void ChasePlayer()
    {
        isInAttackRange = false;
        agent.isStopped = false;
        animator.SetBool("isAttacking", false);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        isInAttackRange = true;
        agent.isStopped = true;

        // Check if the bat can attack (based on cooldown)
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = true;           // Set attacking state
            animator.SetBool("isAttacking", true);
            lastAttackTime = Time.time;
            ShootProjectile();
            Invoke("EndAttack", 1.6f);    // Allow time for the attack to complete (adjust duration as needed)
        }
    }

    private void ShootProjectile()
    {
        projectile.SetActive(true);  
    }

    // Called after attack animation/wind-up finishes
    private void EndAttack()
    {
        isAttacking = false;
    }

    public void AttackSfx()
    {
        audioManager.PlayEnemyBatAttackClip(audioSource);
    }
}
