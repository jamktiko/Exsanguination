using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSwordDamage : MonoBehaviour
{
    [SerializeField] private AapoSwordSwing swordSwing;
    [SerializeField] private Collider mainSwordCollider;
    [SerializeField] private Collider closeRangeCollider;
    [SerializeField] private int damage;
    [SerializeField] private int thirdAttackDamage;

    public bool hasDamagedEnemy = false;

    private void Start()
    {
        // Ensure the colliders trigger OnTriggerEnter
        mainSwordCollider.isTrigger = true;
        closeRangeCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider second = closeRangeCollider;
        if (swordSwing.canDamage && other.CompareTag("Enemy") || second.CompareTag("Enemy") && !hasDamagedEnemy)
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


    private void OnEnable()
    {
        hasDamagedEnemy = false;
    }
}
