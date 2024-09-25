using UnityEngine;

public class AapoSwordSwing : MonoBehaviour
{
    public Animator animator;
    public bool canDamage;
    [SerializeField] private bool canCombo;
    public bool thirdAttackDamage;
    public bool isBlocking;

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

    public void BlockAction()
    {
        animator.SetTrigger("block");
        animator.ResetTrigger("block");

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

    public void ThirdAttackDamage()
    {
        thirdAttackDamage = true;
    }

    public void NormalAttackDamage()
    {
        thirdAttackDamage = false;
    }

    public void Blocking()
    {
        isBlocking = true;
    }

    public void NotBlocking()
    {
        isBlocking = false;
    }
}
