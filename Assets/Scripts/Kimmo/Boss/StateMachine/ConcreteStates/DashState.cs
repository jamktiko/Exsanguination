using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BossAbstractState
{
    private Transform targetWaypoint;

    public DashState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to DASH state.");

        boss.bossAnimator.SetBool("moveForward", true);
        boss.DeactivateBossCollider();
        boss.ChooseWaypoint();
        boss.StartOfDash();
    }

    public override void ExitState()
    {
        base.ExitState();

        boss.bossAnimator.SetBool("moveForward", false);
        boss.ActivateBossCollider();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        boss.RotateTowardsTarget();
        boss.MoveTowardsTarget();
        if (boss.bossTransform.position == new Vector3(boss.targetPosition.x, boss.bossTransform.transform.position.y, boss.targetPosition.z))
        {
            boss.bossStateManager.ChangeState();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
