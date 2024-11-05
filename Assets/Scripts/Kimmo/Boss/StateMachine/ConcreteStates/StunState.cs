using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : BossAbstractState
{
    float stunTimer;

    public StunState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
    {
            
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to STUN state.");

       
        stunTimer = 0;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (stunTimer < boss.stunDuration)
        {
            stunTimer += Time.deltaTime;
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
