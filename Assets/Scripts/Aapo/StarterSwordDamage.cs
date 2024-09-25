using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSwordDamage : MonoBehaviour
{
    [SerializeField] private AapoSwordSwing swordSwing;
    [SerializeField] private int damage;
    [SerializeField] private int thirdAttackDamage;
    private void OnTriggerEnter(Collider other)
    {

        if (swordSwing.canDamage)
        {
            EnemyHealthScript enemyHealthScript = other.GetComponent<EnemyHealthScript>();

            if (swordSwing.thirdAttackDamage)
            {
                enemyHealthScript.ChangeEnemyHealth(thirdAttackDamage);
                Debug.Log("dealt " + thirdAttackDamage + "damage");
            }

            else
                enemyHealthScript.ChangeEnemyHealth(damage);
            Debug.Log("dealt " + damage + "damage");

        }
    }

}
