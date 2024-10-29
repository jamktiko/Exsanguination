using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager
{
    public BossAbstractState currentBossState { get; set; }

    public void Initialize(BossAbstractState startingState)
    {
        currentBossState = startingState;
        currentBossState.EnterState();
    }

    public void ChangeState(BossAbstractState newState)
    {
        currentBossState.ExitState();
        currentBossState = newState;
        currentBossState.EnterState();
    }
}
