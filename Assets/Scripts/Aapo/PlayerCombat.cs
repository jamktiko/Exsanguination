using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    [SerializeField] RuntimeAnimatorController[] weaponAnimators;
    public bool canDamage;
    [SerializeField] private bool canCombo;
    public bool thirdAttackDamage;
    public bool isBlocking;
    private bool blockOnCooldown;
    [SerializeField] private float blockCooldownTime;
    [SerializeField] AudioManager audioManager;
    public int currentWeaponNumber;
    [SerializeField] GameObject starterSword;
    [SerializeField] GameObject slayMore;

    //[SerializeField] private ParticleSystem starterSwordSwing1;
    //[SerializeField] private ParticleSystem starterSwordSwing2;
    //[SerializeField] private ParticleSystem starterSwordSwing3;
    //[SerializeField] private ParticleSystem failedComboVFX;
    //[SerializeField] private ParticleSystem playerIsBlockingVFX;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        animator.runtimeAnimatorController = weaponAnimators[0];
       slayMore.SetActive(false);
    }
   
    private void SetWeaponLogics(int weaponIndex)
    {
        currentWeaponNumber = weaponIndex;
        if (currentWeaponNumber == 0) 
        { 
        animator.runtimeAnimatorController = weaponAnimators[0];
            starterSword.SetActive(true);
            slayMore.SetActive(false);


        }

        if (currentWeaponNumber == 1) 
        {
            animator.runtimeAnimatorController = weaponAnimators[1];
            starterSword.SetActive(false);
            slayMore.SetActive(true);

        }
    }
   
    public void Attack()
    {
        //StarterSword
        if(currentWeaponNumber == 0)
        {
            // Check if the attack just started
            if (!animator.GetBool("startedAttack"))
            {
                audioManager.PlaySwordSwingClips1();
                //starterSwordSwing1.Play();
                animator.SetBool("startedAttack", true);
                // Reset any combo-related states
                animator.SetBool("failedCombo", false);
                return;  // Exit to ensure we only set startedAttack on the first click
            }
            if (canCombo && animator.GetBool("startedAttack"))
            {
                audioManager.PlaySwordSwingClips2();
                //starterSwordSwing2.Play();
                animator.SetBool("isAttacking", true);
                animator.SetBool("failedCombo", false);  // Ensure failedCombo is reset
            }
            // Now handle the combo continuation after the first attack has started
            if (canCombo && animator.GetBool("startedAttack"))
            {
                //starterSwordSwing3.Play();
                audioManager.PlaySwordSwingClips2();
                animator.SetBool("isAttacking", true);
                animator.SetBool("failedCombo", false);  // Ensure failedCombo is reset
            }
            else if (!canCombo && animator.GetBool("startedAttack"))
            {
                //failedComboVFX.Play();
                animator.SetBool("failedCombo", true);
                animator.SetBool("isAttacking", false);  // Stop attacking if failedCombo
            }
        }

        //SlayMore

        if(currentWeaponNumber == 1)
        {
            //SlaymoreanimLogics
        }
        
    }

    public void BlockAction()
    {
        if (!blockOnCooldown)
        {
            //playerIsBlockingVFX.Play();
                animator.SetTrigger("block");
            StartCoroutine(BlockingCooldown());
        }
        

    }

    private IEnumerator BlockingCooldown()
    {
        blockOnCooldown = true;
        yield return new WaitForSeconds(blockCooldownTime);
        blockOnCooldown = false;
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
