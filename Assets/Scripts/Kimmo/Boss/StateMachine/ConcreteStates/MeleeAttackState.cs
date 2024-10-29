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

        Debug.Log("Boss entered to MELEE ATTACK STATE.");

        // Activate boss melee attack animation
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
