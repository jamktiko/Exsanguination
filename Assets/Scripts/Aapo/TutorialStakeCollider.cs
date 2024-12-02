using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStakeCollider : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Collider blockingCollider;

    private void Start()
    {
        blockingCollider = GetComponent<Collider>();
        playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
    }
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
