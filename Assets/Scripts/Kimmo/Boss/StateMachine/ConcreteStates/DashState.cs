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

        boss.DeactivateBossCollider();
        boss.ChooseWaypoint();
    }

    public override void ExitState()
    {
        base.ExitState();

        boss.ActivateBossCollider();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        boss.RotateTowardsTarget();

        boss.MoveTowardsTarget();

        if (boss.transform.position == boss.targetPosition)
        {
            boss.bossStateManager.ChangeState();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
