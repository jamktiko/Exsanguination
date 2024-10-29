using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : BossAbstractState
{
    Transform playerTransform;
    [SerializeField] float moveSpeed;

    public ChargeState(Boss boss, BossStateManager bossStateManager) : base(boss, bossStateManager)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void EnterState()
    {
        base.EnterState();
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

        boss.transform.Translate(playerTransform.position * moveSpeed * Time.deltaTime);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
