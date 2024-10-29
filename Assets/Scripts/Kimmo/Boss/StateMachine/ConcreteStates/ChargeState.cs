using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeState : BossAbstractState
{
    public ChargeState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector3 targetDirection = boss.targetPosition - boss.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(boss.transform.forward, targetDirection, 10f * Time.fixedDeltaTime, 0f);
        boss.transform.rotation = Quaternion.LookRotation(newDirection);

        boss.transform.position = Vector3.MoveTowards(boss.transform.position, boss.targetPosition, 
            boss.moveSpeed * Time.fixedDeltaTime);

        if (boss.transform.position == boss.targetPosition)
        {
            boss.stateManager.ChangeState(boss.meleeAttackState);
        }
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
