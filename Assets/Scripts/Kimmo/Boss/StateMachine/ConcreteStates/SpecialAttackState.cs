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

        boss.bossAnimator.SetTrigger("cast");
        boss.RandomizeSpecialAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!boss.isCastingSpecialAttack)
        {
            boss.bossStateManager.ChangeState();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
