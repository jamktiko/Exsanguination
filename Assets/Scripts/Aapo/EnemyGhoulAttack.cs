using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyGhoulAttack : MonoBehaviour
{
    private StarterSword swordSwing;
    private Animator playerAnimator;
    [SerializeField] int damage;
    private PlayerHealthManager playerHealthManager;
    [SerializeField] private Animator enemyAnimator;


    private void Awake()
    {
        swordSwing = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<StarterSword>();
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (swordSwing.isBlocking)
            {
                enemyAnimator.SetTrigger("stun");
                playerAnimator.SetTrigger("parry");
            }

            else
            {
                
                playerHealthManager.UpdatePlayerHealth(-damage);
            }
        }
    }
}
