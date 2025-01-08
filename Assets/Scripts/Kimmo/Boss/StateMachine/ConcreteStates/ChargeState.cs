using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeState : BossAbstractState
{
    public ChargeState(Boss boss,  BossStateManager bossStateManager) : base(boss, bossStateManager)
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to CHARGE state.");

        boss.bossAnimator.SetBool("moveForward", true);
    }

    public override void ExitState()
    {
        base.ExitState();

        boss.bossAnimator.SetBool("moveForward", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        boss.targetPosition = boss.playerTransform.position;
        boss.RotateTowardsTarget();
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
