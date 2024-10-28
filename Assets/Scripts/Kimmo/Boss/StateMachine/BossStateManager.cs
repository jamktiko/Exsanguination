using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : MonoBehaviour
{
    [SerializeField] BossAbstractState currentState;
    public IdleState idleState;
    public DashState dashState;
    public MeleeAttackState meleeAttackState;
    public SpecialAttackState specialAttackState;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStates();

        SwitchState(meleeAttackState);
        currentState = meleeAttackState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(BossAbstractState state)
    {
        currentState = state;
        state.EnterState();
    }

    public BossAbstractState GetCurrentState() { return currentState; }

    private void InitializeStates()
    {
        idleState = GetComponent<IdleState>();
        dashState = GetComponent<DashState>();
        meleeAttackState = GetComponent<MeleeAttackState>();
        specialAttackState = GetComponent<SpecialAttackState>();
    }
}
