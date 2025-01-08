using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackState : BossAbstractState
{
    public SpecialAttackState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to SPECIAL ATTACK state.");

        
        boss.RandomizeSpecialAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (boss.isCastingSpecialAttack)
        {
            boss.targetPosition = boss.playerTransform.position;
            boss.RotateTowardsTarget();
        }

        AnimatorStateInfo stateInfo = boss.bossAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1.0f && !boss.isCastingSpecialAttack)
        {
            boss.bossStateManager.ChangeState();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
