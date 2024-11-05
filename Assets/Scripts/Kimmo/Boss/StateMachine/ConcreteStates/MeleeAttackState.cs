using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : BossAbstractState
{
    public MeleeAttackState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to MELEE ATTACK state.");

        boss.bossAnimator.SetTrigger("meleeAttack");
        boss.ActivateSwordCollider();
    }

    public override void ExitState()
    {
        base.ExitState();
        boss.DeactivateSwordCollider();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        //boss.bossStateManager.ChangeState();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
