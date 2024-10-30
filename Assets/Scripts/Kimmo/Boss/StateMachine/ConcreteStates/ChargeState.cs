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

        boss.targetPosition = boss.playerTransform.position;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        boss.RotateTowardsTarget();

        boss.MoveTowardsTarget();

        if (boss.transform.position == boss.targetPosition)
        {
            boss.stateManager.ChangeState();
        }
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
