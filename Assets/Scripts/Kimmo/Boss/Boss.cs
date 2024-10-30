using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossMovement bossMovement { get; set; }
    public BossStateManager stateManager { get; set; }
    public IdleState idleState { get; set; }
    public DashState dashState { get; set; }
    public StunState stunState { get; set; }
    public ChargeState chargeState { get; set; }
    public MeleeAttackState meleeAttackState { get; set; }
    public SpecialAttackState specialAttackState { get; set; }

    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] waypoints;
    public Transform playerTransform;
    public Vector3 targetPosition;
    float minDistance = 5f; // Minimum distance from player to exclude waypoint

    public bool meleeAttackBlocked;
    public bool meleeAttackHit;

    public float stunDuration;

    public float idleDuration;

    private void Awake()
    {
        stateManager = new BossStateManager();

        idleState = new IdleState(this, bossMovement, stateManager);
        dashState = new DashState(this, bossMovement, stateManager);
        stunState = new StunState(this, bossMovement, stateManager);
        chargeState = new ChargeState(this, bossMovement, stateManager);
        meleeAttackState = new MeleeAttackState(this, bossMovement, stateManager);
        specialAttackState = new SpecialAttackState(this, bossMovement, stateManager);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        stateManager.states = new BossAbstractState[] { chargeState, meleeAttackState, stunState, dashState, idleState, dashState, specialAttackState, dashState, idleState, dashState };

        stateManager.Initialize(chargeState);
    }

    private void Update()
    {
        stateManager.currentBossState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateManager.currentBossState.PhysicsUpdate();
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
        Vector3 targetDirection = targetPosition - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 10f * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition,
            moveSpeed * Time.deltaTime);
    }
}
