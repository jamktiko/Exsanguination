using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossAbstractState
{
    float idleTimer;

    public IdleState(Boss boss, BossMovement bossMovement, BossStateManager bossStateManager) : base(boss, bossMovement, bossStateManager)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to IDLE state.");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

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
