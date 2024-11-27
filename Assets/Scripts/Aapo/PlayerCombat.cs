using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    [SerializeField] RuntimeAnimatorController[] weaponAnimators;
    public bool canDamage;
    [SerializeField] private bool canCombo;
    public bool specialDamage;
    public bool isBlocking;
    private bool blockOnCooldown;
    [SerializeField] private float blockCooldownTime;
    AudioManager audioManager;
    public int currentWeaponNumber;
    [SerializeField] GameObject starterSword;
    [SerializeField] GameObject slayMore;
    private bool isPerformingAction;
    public bool isAttacking;
    [SerializeField] StarterSwordDamage starterSwordDamage;
    private EnemyHealthScript[] enemies;
    private Image comboCooldownImage;
    [SerializeField] float comboCooldownDuration;

    //[SerializeField] private ParticleSystem starterSwordSwing1;
    //[SerializeField] private ParticleSystem starterSwordSwing2;
    //[SerializeField] private ParticleSystem starterSwordSwing3;
    //[SerializeField] private ParticleSystem failedComboVFX;
    //[SerializeField] private ParticleSystem playerIsBlockingVFX;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        comboCooldownImage = GameObject.FindGameObjectWithTag("ComboCooldown").GetComponent<Image>();

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new EnemyHealthScript[enemyObjects.Length];
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            enemies[i] = enemyObjects[i].GetComponent<EnemyHealthScript>();
        }
    }
    private void Start()
    {
        animator.runtimeAnimatorController = weaponAnimators[0];
       slayMore.SetActive(false);
        audioManager.PlaySwordEquipClips();
        comboCooldownImage.fillAmount = 0;


    }

    private IEnumerator ComboTimer()
    {
        float elapsed = 0f;
        comboCooldownImage.fillAmount = 1f;

        while (elapsed < comboCooldownDuration)
        {
            elapsed += Time.deltaTime;
            float fillValue = Mathf.Lerp(1f, 0f, elapsed / comboCooldownDuration);
            comboCooldownImage.fillAmount = fillValue;
            yield return null;
        }

        comboCooldownImage.fillAmount = 0f;
    }


    public void DrawSword()
    {
        animator.SetBool("hasDrawn", true);
    }

    public void PerformAction()
    {
        isPerformingAction = true;
    }


    public void StopAction()
    {
        isPerformingAction = false;
    }

    public void FinishAttack()
    {
        isAttacking = false;
    }

    public void AnimatorAttackingFalse()
    {
        animator.SetBool("isAttacking", false);
    }

    public void SetWeaponLogics(int weaponIndex)
    {
        currentWeaponNumber = weaponIndex;
        if (currentWeaponNumber == 0) 
        { 
        animator.runtimeAnimatorController = weaponAnimators[0];
            starterSword.SetActive(true);
            slayMore.SetActive(false);
            audioManager.PlaySwordEquipClips();

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
        if (isPerformingAction || isBlocking) return; // Prevent action if another is in progress
        isAttacking = true;
        //StarterSword
        if (currentWeaponNumber == 0)
        {
            // Check if the attack just started
            if (!animator.GetBool("startedAttack"))
            {
                //starterSwordSwing1.Play();
                animator.SetBool("startedAttack", true);
                // Reset any combo-related states
                animator.SetBool("failedCombo", false);
                return;  // Exit to ensure we only set startedAttack on the first click
            }
            if (canCombo && animator.GetBool("startedAttack"))
            {
                //starterSwordSwing2.Play();
                animator.SetBool("isAttacking", true);
                animator.SetBool("failedCombo", false);  // Ensure failedCombo is reset
            }
            // Now handle the combo continuation after the first attack has started
            if (canCombo && animator.GetBool("startedAttack"))
            {
                //starterSwordSwing3.Play();
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
            animator.SetTrigger("Attack");
        }
        
    }

    public void BlockAction()
    {
        if (isAttacking || blockOnCooldown) return; // Prevent blocking if another action is in progress
        
            //playerIsBlockingVFX.Play();
            animator.SetTrigger("block");
        isBlocking = true;
            StartCoroutine(BlockingCooldown());
        

        if (currentWeaponNumber == 1)
        {
            animator.SetTrigger("SecondAttack");
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


        foreach (var enemy in enemies)
        {
            if (enemy != null) // Ensure the enemy hasn't been destroyed
            {
                enemy.hasBeenDamaged = false;
            }
        }
    }

    public void CanCombo()
    {
        if (!animator.GetBool("failedCombo"))
        {
            canCombo = true;
            StartCoroutine(ComboTimer());
        }
    }

    public void CantCombo()
    {
        canCombo = false;

        //animator.SetBool("isAttacking", false);


    }

    public void ThirdAttackDamage()
    {
        specialDamage = true;
    }

    public void NormalAttackDamage()
    {
        specialDamage = false;
    }

    public void Blocking()
    {
        isBlocking = true;
    }

    public void NotBlocking()
    {
        isBlocking = false;
    }

    public void Attack1Sfx()
    {
        audioManager.PlaySwordSwingClips1();

    }

    public void Attack2Sfx()
    {
        audioManager.PlaySwordSwingClips2();
    }

    public void Attack3Sfx()
    {
        audioManager.PlaySwordSwingClips3();

    }
}



