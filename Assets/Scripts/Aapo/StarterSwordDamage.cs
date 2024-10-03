using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSwordDamage : MonoBehaviour
{
    [SerializeField] private StarterSword swordSwing;
    [SerializeField] private int damage;
    [SerializeField] private int thirdAttackDamage;

    public bool hasDamagedEnemy = false;

   

    private void OnTriggerEnter(Collider other)
    {
       
        if (swordSwing.canDamage && other.CompareTag("Enemy"))
        {
            Debug.Log("hit enemy");
            EnemyHealthScript enemyHealthScript = other.GetComponent<EnemyHealthScript>();

            if (swordSwing.thirdAttackDamage)
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
