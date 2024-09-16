using UnityEngine;

public class AapoSwordSwing : MonoBehaviour
{

    private Animator animator;
    public bool canAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void SwingSword()
    {
        if (canAttack)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
       
    }

    public void CanAttack()
    {
        canAttack = true;
   }
    public void CantAttack()
    {
        canAttack = false;
    }


}
