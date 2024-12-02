using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStakeCollider : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Collider collider;
    
    // Update is called once per frame
    void Update()
    {
        if (playerStats.foundStake) 
        { 
        collider.enabled = false;
            this.enabled = false;
        }
    }
}
