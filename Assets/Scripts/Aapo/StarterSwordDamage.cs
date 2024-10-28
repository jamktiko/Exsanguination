using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSwordDamage : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private int damage;
    [SerializeField] private int thirdAttackDamage;

    public bool hasDamagedEnemy = false;

    private void OnTriggerEnter(Collider other)
    {
        if (playerCombat.canDamage && other.CompareTag("Enemy"))
        {
            
                EnemyHealthScript enemyHealthScript = other.GetComponent<EnemyHealthScript>();

                if (playerCombat.specialDamage)
                {
                    enemyHealthScript.ChangeEnemyHealth(thirdAttackDamage);
                    Debug.Log("Dealt " + thirdAttackDamage + " damage");
                }
                else
                {
                    enemyHealthScript.ChangeEnemyHealth(damage);
                    Debug.Log("Dealt " + damage + " damage");
                }

                hasDamagedEnemy = true;
            }
        
    }

   
}
