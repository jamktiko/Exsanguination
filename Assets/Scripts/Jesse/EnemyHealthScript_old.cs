//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class EnemyHealthScript_old : MonoBehaviour
//{
//    [SerializeField] int maxHealth = 100;
//    [SerializeField] int health = 100;
//    [SerializeField] float finisherTime = 1f; //seconds how long does the finisher take until enemy dies
//    private StakeLogic stakeLogic;
//    AudioManager audioManager;
//    [SerializeField] HealthBarScript healthBar;
//   //SerializeField] StakeLogic stakeLogic;
//    [SerializeField] private GameObject stake;
//    [SerializeField] private GameObject playerEnemyFinisherSlot;
//    [SerializeField] Animator playerAnimator;
//    public bool isBeingFinished;
//    [SerializeField] RBInputManager rbInputManager;
//    [SerializeField] Camera playercamera;
//    public GameObject thisgameObject;
//    [SerializeField] SkinnedMeshRenderer enemyMesh;
//    private Quaternion startRotation = Quaternion.Euler(-50f, 0, 0);
//    private Quaternion endRotation = Quaternion.Euler(0, 0, 0);
//    private float lerpDuration = 1.5f;


//    void Awake()
//    {
//        healthBar = GetComponentInChildren<HealthBarScript>();
//       stakeLogic = GameObject.FindGameObjectWithTag("Stake").GetComponent<StakeLogic>();
//    }

//    private void Start()
//    {
//        healthBar.UpdateHealthBar(health, maxHealth);
//        health = maxHealth;
//    }

   
    
//    public void ChangeEnemyHealth(int changeAmount)
//    {
//        audioManager.PlayEnemyTakeDamageAudioClip();
//        health = Mathf.Clamp(health - changeAmount, 0, maxHealth);
//        healthBar.UpdateHealthBar(health, maxHealth);
//        if (health <= 0)
//        {
//            Die();        
//        }
//    }

//    // Get the enemy's current health
//    public int GetEnemyHealth()
//    {
//        return health;
//    }
//    public int GetEnemyMaxHealth()
//    {
//        return maxHealth;
//    }

//    public void Finish()
//    {
//        // Trigger death animation or effects here
//        //stakeLogic.UnstickFromEnemy();
//        thisgameObject.layer = LayerMask.NameToLayer("IgnoreAll");
//        Rigidbody rb = thisgameObject.GetComponent<Rigidbody>();
//        rb.isKinematic = true;
//        Debug.Log("finisher started");
//        isBeingFinished = true;
//        ResetEnemyAndStickTransforms();
//        playerAnimator.SetTrigger("finish");
//        //player visual effects
//        StartCoroutine(FinisherTimer());
//    }

//    private void ResetEnemyAndStickTransforms()
//    {
//        thisgameObject.transform.SetParent(playerEnemyFinisherSlot.transform);
//        thisgameObject.transform.localPosition = Vector3.zero;
//        thisgameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
//        stake.transform.localPosition = Vector3.zero;
//        //stake.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
//    }

//    private IEnumerator FinisherTimer()
//    {

//        // Ensure the final rotation is exactly the end rotation at the end of the coroutine
//        playercamera.transform.localRotation = startRotation;
//        rbInputManager.ControlsEnabled(false);
//        yield return new WaitForSeconds(finisherTime);
//        enemyMesh.enabled = false;
//        playercamera.transform.localRotation = endRotation;
//        yield return new WaitForSeconds(0.5f);
//        rbInputManager.ControlsEnabled(true);
//        stakeLogic.ReturnToPlayer();
//        Die();
//    }

   


//// Die and destroy the enemy
//private void Die()
//    {
//        // Handle enemy death logic here
//        //audioManager.PlayEnemyDieAudioClip();
//        Destroy(thisgameObject);
//        Debug.Log("enemy died");
//    }
//}
