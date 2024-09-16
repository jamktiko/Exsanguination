using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    //HealthScript healthScript;
    [SerializeField] int damage;
    [SerializeField] bool canDamage;

    public void CanDamage()
    {
        canDamage = true;
    }

    public void CannotDamage()
    {
        canDamage = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && canDamage)
        {
            EnemyHealthScript healthScript = collision.GetComponent<EnemyHealthScript>();
            
            if (healthScript != null)
            {
                Debug.Log(healthScript);
                healthScript.ChangeHealth(damage);
            }
            
        }
    }
}
