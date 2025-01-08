using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAbstractState
{
    protected Boss boss;
    protected BossStateManager bossStateManager;

    public BossAbstractState(Boss boss, BossStateManager bossStateManager)
    {
        this.boss = boss;
        this.bossStateManager = bossStateManager;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
}
