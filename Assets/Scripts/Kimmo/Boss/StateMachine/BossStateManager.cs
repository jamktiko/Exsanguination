using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BossStateManager
{
    protected Boss boss;
    public BossAbstractState currentBossState { get; set; }

    public BossAbstractState[] states;
    int stateIndex;
    public bool isDead;

    private void Start()
    {
        
    }

    public void Initialize(BossAbstractState startingState)
    {
        currentBossState = startingState;
        currentBossState.EnterState();
    }

    public void ChangeState()
    {
        currentBossState.ExitState();
        if (isDead) return;

        if (stateIndex < states.Length - 1)
        {
            stateIndex++;
        }
        else
        {
            stateIndex = 0;
        }

        currentBossState = states[stateIndex];

        currentBossState.EnterState();
    }
}
