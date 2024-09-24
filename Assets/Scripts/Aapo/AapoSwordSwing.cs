using UnityEngine;

public class AapoSwordSwing : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool canDamage;
    [SerializeField] private int damage;
    [SerializeField] private bool canCombo;
 

    private void OnTriggerEnter(Collider other)
    {
        if (canDamage)
        {
            EnemyHealthScript enemyHealthScript = other.GetComponent<EnemyHealthScript>();

            enemyHealthScript.ChangeEnemyHealth(damage);
        }
    }



    public void ContinueCombo()
    {
        // Check if the attack just started
        if (!animator.GetBool("startedAttack"))
        {
            animator.SetBool("startedAttack", true);
            // Reset any combo-related states
            animator.SetBool("failedCombo", false);
            return;  // Exit to ensure we only set startedAttack on the first click
        }

        // Now handle the combo continuation after the first attack has started
        if (canCombo && animator.GetBool("startedAttack"))
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("failedCombo", false);  // Ensure failedCombo is reset
        }
        else if (!canCombo && animator.GetBool("startedAttack"))
        {
            animator.SetBool("failedCombo", true);
            animator.SetBool("isAttacking", false);  // Stop attacking if failedCombo
        }
    }

    public void StopCombo()
    {
        canCombo = false;
        animator.SetBool("isAttacking", false);
        animator.SetBool("startedAttack", false);
        animator.SetBool("failedCombo", false);


    }

    public void CanDamageMethod()
    {
        canDamage = true;
    }

    public void CantDamageMethod()
    {
        canDamage = false;
    }

    public void CanCombo()
    {
        if (!animator.GetBool("failedCombo"))
        {
            canCombo = true;
        }
    }

    public void CantCombo()
    {
        canCombo = false;
        animator.SetBool("isAttacking", false);


    }

}
