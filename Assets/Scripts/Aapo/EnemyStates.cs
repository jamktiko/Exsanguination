using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    public bool isStunned;

    public void StunStart()
    {
        isStunned = true;
    }

    public void StunEnd()
    {
        isStunned = false;

    }
}
