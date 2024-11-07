using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossAbstractState
{
    float idleTimer;
    string[] animationTriggers = new string[] { "stalkingRight", "stalkingLeft"  };
    string currentAnimationTrigger;

    public IdleState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Boss entered to IDLE state.");

        currentAnimationTrigger = animationTriggers[Random.Range(0, animationTriggers.Length)];
        boss.bossAnimator.SetTrigger(currentAnimationTrigger);
    }

    public override void ExitState()
    {
        base.ExitState();
        idleTimer = 0;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        boss.targetPosition = boss.playerTransform.position;
        boss.RotateTowardsTarget();
        boss.MoveSideways();

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
