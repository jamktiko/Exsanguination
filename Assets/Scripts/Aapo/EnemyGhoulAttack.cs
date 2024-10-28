using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyGhoulAttack : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private Animator playerAnimator;
    [SerializeField] int damage;
    private PlayerHealthManager playerHealthManager;
    AudioManager audioManager;
    [SerializeField] private Animator enemyAnimator;


    private void Awake()
    {
        playerCombat = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<PlayerCombat>();
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //if(playerCombat.currentWeaponNumber == 0)
            //{
                if (playerCombat.isBlocking)
                {
                    audioManager.PlayParryAudioClip();
                    enemyAnimator.SetTrigger("stun");
                    playerAnimator.SetTrigger("parry");
                }
            //}
            

            else
            {
                audioManager.PlayPlayerTakeDamageAudioClip();
                playerHealthManager.UpdatePlayerHealth(-damage);
            }
        }
    }
}
