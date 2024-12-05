using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    [SerializeField] int damage;
    PlayerHealthManager playerHealthManager;
    bool canDamage = true;
    bool isTouchingPlayer;
    
    private void Awake()
    {
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isTouchingPlayer = true;
 
            Damage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTouchingPlayer = false;
        }
    }

    private void Damage()
    {
        canDamage = false;
        playerHealthManager.UpdatePlayerHealth(-damage);
        StartCoroutine(WaitBeforeNextDamage());
    }

    IEnumerator WaitBeforeNextDamage()
    {
        yield return new WaitForSeconds(1);
        canDamage = true;

        if (isTouchingPlayer)
        {
            Damage();
        }
    }
}
