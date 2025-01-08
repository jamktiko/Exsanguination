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

    [SerializeField] private ParticleSystem starterSwordSwing1;
    [SerializeField] private ParticleSystem starterSwordSwing2;
    [SerializeField] private ParticleSystem starterSwordSwing3;
    //[SerializeField] private ParticleSystem failedComboVFX;
    //[SerializeField] private ParticleSystem playerIsBlockingVFX;
    public int comboStep;
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
        comboStep = 0; // Reset combo back to the start

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

        // StarterSword
        if (currentWeaponNumber == 0)
        {
            // Step 1: Check if starting a new attack sequence
            if (!animator.GetBool("isAttacking") && comboStep == 0)
            {
                animator.SetBool("startedAttack", true);
                animator.SetBool("isAttacking", true);
                animator.SetBool("failedCombo", false); // Reset combo failure state

                // Play the particle effect for the first swing
                Debug.Log("First Swing");

                comboStep = 1; // Move to the second step in the combo
                animator.SetInteger("comboStep", comboStep);
                return; // Exit to ensure the first swing finishes before checking others
            }

            // Step 2: Handle combo progression based on `comboStep`
            if (canCombo && comboStep == 1)
            {
                Debug.Log("Second Swing");

                comboStep = 2; // Move to the third step in the combo
                animator.SetBool("isAttacking", true);
                animator.SetInteger("comboStep", comboStep);
                return; // Exit to prevent triggering multiple swings at once
            }

            if (canCombo && comboStep == 2)
            {
                Debug.Log("Third Swing");

                comboStep = 3;
                animator.SetBool("isAttacking", true);
                animator.SetInteger("comboStep", comboStep);
                return; // Exit to prevent triggering multiple swings at once
            }

            if (canCombo && comboStep == 3)
            {
                // Reset combo after the final attack
                Debug.Log("Reset Combo");
                comboStep = 0;
                animator.SetBool("isAttacking", true);
                animator.SetInteger("comboStep", comboStep);
                return; // Exit to prevent triggering multiple swings at once
            }

            // Step 3: Handle failed combo scenario
            if (!canCombo)
            {
                animator.SetBool("failedCombo", true);
                animator.SetBool("isAttacking", false); // Stop attacking if combo fails
                Debug.Log("Failed Combo");
            }
        }

        // SlayMore
        if (currentWeaponNumber == 1)
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
        //starterSwordSwing1.Play();
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
        // Reset combo step and animator states after the combo is finished
        


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

    public void Attack1Fx()
    {
        audioManager.PlaySwordSwingClips1();
        starterSwordSwing1.Play();

    }

    public void Attack2Fx()
    {
        audioManager.PlaySwordSwingClips2();
        starterSwordSwing2.Play();

    }

    public void Attack3Fx()
    {
        audioManager.PlaySwordSwingClips3();
        starterSwordSwing3.Play();


    }

    public void ParryFx()
    {
        audioManager.PlaySwordSwingClips3();
        //vfx for parry
    }


}



