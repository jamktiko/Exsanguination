using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IMoveable
{
    public BossStateManager stateManager { get; set; }
    public IdleState idleState { get; set; }
    public DashState dashState { get; set; }
    public StunState stunState { get; set; }
    public ChargeState chargeState { get; set; }
    public MeleeAttackState meleeAttackState { get; set; }
    public SpecialAttackState specialAttackState { get; set; }
    public float moveSpeed { get; set; }

    private void Awake()
    {
        stateManager = new BossStateManager();

        idleState = new IdleState(this, stateManager);
        dashState = new DashState(this, stateManager);
        stunState = new StunState(this, stateManager);
        chargeState = new ChargeState(this, stateManager);
        meleeAttackState = new MeleeAttackState(this, stateManager);
        specialAttackState = new SpecialAttackState(this, stateManager);
    }

    private void Start()
    {
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

    public void MoveBoss(Vector3 velocity)
    {
        
    }
}
