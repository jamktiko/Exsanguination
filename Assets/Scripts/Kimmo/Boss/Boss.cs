using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossStateManager stateManager { get; set; }
    public IdleState idleState { get; set; }
    public DashState dashState { get; set; }
    public StunState stunState { get; set; }
    public ChargeState chargeState { get; set; }
    public MeleeAttackState meleeAttackState { get; set; }
    public SpecialAttackState specialAttackState { get; set; }

    public float moveSpeed;
    public Transform[] wayPoints;
    public Transform playerTransform;
    public Vector3 targetPosition;


    private void Awake()
    {
        stateManager = new BossStateManager();

        idleState = new IdleState(this, stateManager);
        dashState = new DashState(this, stateManager);
        stunState = new StunState(this, stateManager);
        chargeState = new ChargeState(this, stateManager);
        meleeAttackState = new MeleeAttackState(this, stateManager);
        specialAttackState = new SpecialAttackState(this, stateManager);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        stateManager.Initialize(dashState);
    }

    private void Update()
    {
        stateManager.currentBossState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateManager.currentBossState.PhysicsUpdate();
    }
}
