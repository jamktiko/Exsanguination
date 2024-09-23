using UnityEngine;

public class AapoSwordSwing : MonoBehaviour
{

    [SerializeField] private Animator animator;
    public bool canAttack;

  
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
