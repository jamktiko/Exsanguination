using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGhoul : MonoBehaviour
{
    
 
    [SerializeField] TutorialCombat tutorialCombat;
    [SerializeField] bool isMelee;
        [SerializeField] bool isParry;
    [SerializeField] bool isFinishable;
    public bool meleeIsDead;
    public bool parriedCorrectly;
    public bool finished;
    [SerializeField] EnemyStates enemyStates;
    [SerializeField] EnemyFinisher enemyFinisher;
    [SerializeField] GameObject meleeGhoulObject;
    [SerializeField] Animator parryGhoulAnimator;
    // Start is called before the first frame update
    void Start()
    {
        if (isMelee)
        {
            Debug.Log("melee is enabled");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isParry)
        {
            if (enemyStates.isStunned)
            {
                parriedCorrectly = true;
            }
        }
        if (isMelee && !meleeGhoulObject.activeSelf)
        {
            parryGhoulAnimator.SetTrigger("detect");
                meleeIsDead = true;
            
        }

        if (isFinishable)
        {
            if (enemyFinisher.isFinishing)
            {
                finished = true;
            }
        }
    }

   
}
