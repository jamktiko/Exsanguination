using UnityEngine;

public class SlayMoreDamage : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (playerCombat.canDamage && other.CompareTag("Enemy"))
        {
            EnemyHealthScript enemyHealthScript = other.GetComponent<EnemyHealthScript>();
                enemyHealthScript.ChangeEnemyHealth(damage);
        }

    }
}
