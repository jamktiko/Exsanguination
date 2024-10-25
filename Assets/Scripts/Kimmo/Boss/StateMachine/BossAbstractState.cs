using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAbstractState : MonoBehaviour
{
    protected BossStateManager bossStateManager;

    // Start is called before the first frame update
    public virtual void Start()
    {
        bossStateManager = GetComponent<BossStateManager>();
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ChangeToNextState();
}
