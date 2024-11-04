using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossMovement bossMovement { get; set; }
    public BossStateManager bossStateManager { get; set; }
    public IdleState idleState { get; set; }
    public DashState dashState { get; set; }
    public StunState stunState { get; set; }
    public ChargeState chargeState { get; set; }
    public MeleeAttackState meleeAttackState { get; set; }
    public SpecialAttackState specialAttackState { get; set; }

    [SerializeField] Collider bossCollider;
    [SerializeField] Collider swordCollider;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] waypoints;
    [SerializeField] Transform bossTransform;
    public Transform playerTransform;
    public Vector3 targetPosition;
    float minDistance = 5f; // Minimum distance from player to exclude waypoint
    public bool isInMeleeRange;

    public Animator bossAnimator;

    public float stunDuration;

    public float idleDuration;

    private void Awake()
    {
        bossStateManager = new BossStateManager();

        idleState = new IdleState(this, bossMovement, bossStateManager);
        dashState = new DashState(this, bossMovement, bossStateManager);
        stunState = new StunState(this, bossMovement, bossStateManager);
        chargeState = new ChargeState(this, bossMovement, bossStateManager);
        meleeAttackState = new MeleeAttackState(this, bossMovement, bossStateManager);
        specialAttackState = new SpecialAttackState(this, bossMovement, bossStateManager);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        bossStateManager.states = new BossAbstractState[] { chargeState, meleeAttackState, stunState, dashState, idleState, dashState, specialAttackState, dashState, idleState, dashState };

        bossStateManager.Initialize(bossStateManager.states[0]);
    }

    private void Update()
    {
        bossStateManager.currentBossState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        bossStateManager.currentBossState.PhysicsUpdate();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isInMeleeRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInMeleeRange = false;
        }
    }

    public void DeactivateBossCollider()
    {
        bossCollider.enabled = false;
    }

    public void ActivateBossCollider()
    {
        bossCollider.enabled = true;
    }

    public void DeactivateSwordCollider()
    {
        swordCollider.enabled = false;
    }

    public void ActivateSwordCollider()
    {
        swordCollider.enabled = true;
    }

    public void ChooseWaypoint()
    {
        List<Transform> availableWaypoints = new List<Transform>();

        foreach (Transform waypoint in waypoints)
        {
            if (Vector3.Distance(waypoint.position, playerTransform.position) >= minDistance)
            {
                availableWaypoints.Add(waypoint);
            }
        }

        targetPosition = availableWaypoints[Random.Range(0, availableWaypoints.Count)].position;
    }

    public void RotateTowardsTarget()
    {
        Debug.Log("Boss rotates");
        Vector3 targetDirection = targetPosition - bossTransform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 10f * Time.deltaTime, 0f);
        bossTransform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void MoveTowardsTarget()
    {
        Debug.Log("Boss moves");
        bossTransform.position = Vector3.MoveTowards(bossTransform.position, targetPosition,
            moveSpeed * Time.deltaTime);
    }
}
