using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AapoEnemyAttack : MonoBehaviour
{
    private AapoSwordSwing swordSwing;
    private Animator playerAnimator;
    [SerializeField] int damage;
    private PlayerHealthManager playerHealthManager;



    private void Awake()
    {
        swordSwing = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<AapoSwordSwing>();
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (swordSwing.isBlocking)
            {
                playerAnimator.SetTrigger("parry");
            }

            else
            {
                
                playerHealthManager.UpdatePlayerHealth(-damage);
            }
        }
    }
}
