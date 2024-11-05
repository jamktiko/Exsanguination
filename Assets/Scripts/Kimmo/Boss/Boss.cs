using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss : MonoBehaviour
{
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
    [SerializeField] float rotationSpeed;
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

        idleState = new IdleState(this, bossStateManager);
        dashState = new DashState(this, bossStateManager);
        stunState = new StunState(this, bossStateManager);
        chargeState = new ChargeState(this, bossStateManager);
        meleeAttackState = new MeleeAttackState(this, bossStateManager);
        specialAttackState = new SpecialAttackState(this, bossStateManager);

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
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            bossTransform.rotation = Quaternion.Slerp(bossTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void MoveTowardsTarget()
    {
        var actualTargetPosition = new Vector3(targetPosition.x, 0, targetPosition.z);
        bossTransform.position = Vector3.MoveTowards(bossTransform.position, actualTargetPosition,
            moveSpeed * Time.deltaTime);
    }
}
