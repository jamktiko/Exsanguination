using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeState : BossAbstractState
{
    public ChargeState(Boss boss, BossMovement bossMovement, BossStateManager bossStateManager) : base(boss, bossMovement, bossStateManager)
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to CHARGE state.");

        boss.DeactivateCollider();
        
    }

    public override void ExitState()
    {
        base.ExitState();

        boss.ActivateCollider();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        boss.RotateTowardsTarget();

        boss.targetPosition = boss.playerTransform.position;
        boss.MoveTowardsTarget();

        if (boss.isInMeleeRange)
        {
            boss.bossStateManager.ChangeState();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();  
    }
}
