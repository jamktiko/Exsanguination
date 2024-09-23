using UnityEngine;

public class AapoSwordSwing : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private int comboStep = 0;
    private bool isAttacking = false;
    private float attackStartTime = 0f;
    public float[] attackDurations = { 2.0f, 1.5f, 2.75f }; // Set each attack duration (time in seconds)
    public float comboWindow = 5f; // The time allowed to chain attacks

    public void SwingSword()
    {
        Debug.Log("SwingSword called");
        if (!isAttacking)
        {
            isAttacking = true;
            comboStep = 1; // Start with the first attack
            attackStartTime = Time.time;
            PlayAttackAnimation();
        }
    }

    public void ContinueCombo()
    {
        Debug.Log("ContinueCombo called, current comboStep: " + comboStep);
        if (isAttacking && comboStep < attackDurations.Length)
        {
            float elapsed = Time.time - attackStartTime;
            Debug.Log("Elapsed time: " + elapsed); // Log elapsed time

            if (elapsed >= attackDurations[comboStep - 1])
            {
                comboStep++;
                attackStartTime = Time.time;
                PlayAttackAnimation();
            }
        }
        else
        {
            StopCombo();
        }
    }

    private void PlayAttackAnimation()
    {
        Debug.Log("Setting comboStep in Animator to: " + comboStep);
        animator.SetInteger("comboStep", comboStep);
    }

    public void StopCombo()
    {
        Debug.Log("Stopping combo");
        isAttacking = false;
        comboStep = 0; // Reset the combo
        animator.SetInteger("comboStep", comboStep); // Optionally reset the animator
        // animator.Play("Idle"); // Optionally return to Idle or another animation
    }

    public void CanAttack()
    {
        isAttacking = false; // Allow starting the combo again
    }

    public void CantAttack()
    {
        isAttacking = true; // Prevent starting a new attack
    }
}
