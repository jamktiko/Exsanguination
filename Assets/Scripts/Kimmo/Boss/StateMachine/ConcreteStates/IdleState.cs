using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossAbstractState
{
    float idleTimer;

    public IdleState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to IDLE state.");

        boss.bossAnimator.SetTrigger("idle");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        boss.targetPosition = boss.playerTransform.position;
        boss.RotateTowardsTarget();

        if (idleTimer < boss.idleDuration)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            boss.bossStateManager.ChangeState();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
