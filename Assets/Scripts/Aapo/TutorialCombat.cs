using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialCombat : MonoBehaviour
{
    
    [SerializeField] GameObject meleeGhoul;
    [SerializeField] GameObject parryGhoul;
    [SerializeField] GameObject finisherGhoul;
    [SerializeField] GameObject pickableStake;
    [SerializeField] GameObject meleeUI;
    [SerializeField] GameObject parryUI;
    [SerializeField] GameObject finisherUI;
    [SerializeField] GameObject doneUI;
    [SerializeField] Animator tutorialDoorAnimator;
    [SerializeField] TutorialGhoul meleeTutorialGhoul;
    [SerializeField] TutorialGhoul parryTutorialGhoul;
    [SerializeField] TutorialGhoul finisherTutorialGhoul;
    [SerializeField] StarterSwordDamage starterSwordDamage;
    [SerializeField] EnemyHealthScript finisherHealthScript;
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioSource doorOpenAudio;
    private int tutorialStep;
    
    // Start is called before the first frame update
    void Start()
    {
        meleeGhoul.SetActive(true);
        parryGhoul.SetActive(false);
        finisherGhoul.SetActive(false);
        pickableStake.SetActive(false);
        meleeUI.SetActive(false);
        parryUI.SetActive(false);
        finisherUI.SetActive(false);
        doneUI.SetActive(false);
        tutorialStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (meleeTutorialGhoul.meleeIsDead && tutorialStep == 0)
        {
            SpawnParry();
            
        }
        if (parryTutorialGhoul.parriedCorrectly && tutorialStep == 1)
        {

            SpawnFinishable();
        }

        if (finisherTutorialGhoul.finished && tutorialStep == 2)
        {
            Debug.Log("isFinished");
            FinishTutorial();
        }

        if(finisherGhoul.activeSelf && finisherHealthScript.health < 19)
        {
            starterSwordDamage.damage = 0;
            starterSwordDamage.thirdAttackDamage = 0;
        }

    }

    void SpawnParry()
    {
        starterSwordDamage.damage = 0;
        starterSwordDamage.thirdAttackDamage = 30;
        Debug.Log("spawned parry");
        tutorialStep = 1;
        meleeUI.SetActive(false);
        parryUI.SetActive(true);
        StartCoroutine(waitFewSeconds(1, parryGhoul));
    }

    void SpawnFinishable()
    {
        starterSwordDamage.damage = 15;
        starterSwordDamage.thirdAttackDamage = 30;
        Debug.Log("spawned finishable");
        tutorialStep = 2;
        parryUI.SetActive(false);
        finisherUI.SetActive(true);
        pickableStake.SetActive(true);
        StartCoroutine(waitFewSeconds(2, finisherGhoul));
    }

    void FinishTutorial() 
    {
        starterSwordDamage.damage = 15;
        starterSwordDamage.thirdAttackDamage = 30;
        finisherUI.SetActive(false);
        doneUI.SetActive(true);
        tutorialStep = 3;
        tutorialDoorAnimator.SetTrigger("open");
        doorOpenAudio.Play();
    }

    private IEnumerator waitFewSeconds(int tutorialStepNumber, GameObject ghoulToTurnOn)
    {
        tutorialStep = tutorialStepNumber;       
        yield return new WaitForSeconds(3);
        ghoulToTurnOn.SetActive(true);
        audioManager.PlayEnemyAlertAudioClip(ghoulToTurnOn.GetComponentInChildren<AudioSource>());

    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Player")
        {
            if (tutorialStep == 0)
            {
                meleeUI.SetActive(true);
            }

        }
        
    }
}
