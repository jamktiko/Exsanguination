using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BossAbstractState
{
    Vector3 initialPosition;

    public DeathState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to DEATH state.");

        boss.bossAnimator.SetTrigger("isDying");
        boss.ShowVictoryScreen();
        initialPosition = boss.bossTransform.position;
    }

    public override void ExitState()
    {
        base.ExitState();
        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        boss.bossTransform.position = initialPosition;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
