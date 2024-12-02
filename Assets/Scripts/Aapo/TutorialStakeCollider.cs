using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStakeCollider : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Collider blockingCollider;
    
    // Update is called once per frame
    void Update()
    {
        if (playerStats.foundStake) 
        {
            blockingCollider.enabled = false;
            this.enabled = false;
        }
    }
}
