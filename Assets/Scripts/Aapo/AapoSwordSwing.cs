using UnityEngine;

public class AapoSwordSwing : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool isAttacking = false;



    public void SwingSword()
    {
        Debug.Log("SwingSword called");
        if (!isAttacking)
        {
            isAttacking = true;
        }
    }

    public void ContinueCombo()
    {
        
            isAttacking = true;
        
    }



    public void StopCombo()
    {
        isAttacking = false;
    }

}
