using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

        AnimatorStateInfo stateInfo = boss.bossAnimator.GetCurrentAnimatorStateInfo(0);


        if (stateInfo.IsName("1H Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            boss.bossStateManager.ChangeState();
        }
        else if (stateInfo.IsName("Front Hit Large Reaction"))
        {
            boss.isStunned = true;
            boss.bossStateManager.ChangeState();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
