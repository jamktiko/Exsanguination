using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BossAbstractState
{
    private Transform targetWaypoint;

    public DashState(Boss boss, BossMovement bossMovement, BossStateManager bossStateManager) : base(boss, bossMovement, bossStateManager)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to DASH state.");
        
        boss.ChooseWaypoint();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        boss.RotateTowardsTarget();

        boss.MoveTowardsTarget();

        if (boss.transform.position == boss.targetPosition)
        {
            boss.stateManager.ChangeState();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
