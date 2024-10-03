using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;
    [SerializeField] float finisherTime = 1f; //seconds how long does the finisher take until enemy dies
    private StakeLogic stakeLogic;
    [SerializeField] AudioManager audioManager;
    [SerializeField] HealthBarScript healthBar;
   //SerializeField] StakeLogic stakeLogic;
    [SerializeField] private GameObject stake;
    [SerializeField] private GameObject playerEnemyFinisherSlot;
    [SerializeField] Animator playerAnimator;
    public bool isBeingFinished;
    [SerializeField] InputManager rbInputManager;
    [SerializeField] Transform playerCamera;
    public GameObject thisgameObject;
    private Quaternion startRotation = Quaternion.Euler(-50f, 0, 0);
    private Quaternion endRotation = Quaternion.Euler(0, 0, 0);
    public float lerpToEndDuration = 0.2f; // Duration to lerp to the end rotation
    public float stayDuration = 0.8f;       // Duration to stay at the end rotation
    public float lerpToStartDuration = 0.5f; // Duration to lerp back to the start rotation
    public GameObject ghoulFinishedEnemy;


    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBarScript>();
       stakeLogic = GameObject.FindGameObjectWithTag("Stake").GetComponent<StakeLogic>();
    }

    private void Start()
    {
        ghoulFinishedEnemy.SetActive(false);
        healthBar.UpdateHealthBar(health, maxHealth);
        health = maxHealth;
    }

    private void Update()
    {
        
    }

    public void ChangeEnemyHealth(int changeAmount)
    {
        audioManager.PlayEnemyTakeDamageAudioClip();
        health = Mathf.Clamp(health - changeAmount, 0, maxHealth);
        //healthBar.UpdateHealthBar(health, maxHealth);
        Debug.Log("Enemy health = " + health);
        if (health <= 0)
        {
            Debug.Log("Enemy health is zero");
            Die();
        }
    }

    // Get the enemy's current health
    public int GetEnemyHealth()
    {
        return health;
    }
    public int GetEnemyMaxHealth()
    {
        return maxHealth;
    }

    public void Finish()
    {
        // Trigger death animation or effects here
        stakeLogic.UnstickFromEnemy();
        stakeLogic.ReturnToPlayer();
        Debug.Log("finisher started");
        isBeingFinished = true;
        playerAnimator.SetTrigger("finish");
        //player visual effects
        StartCoroutine(ShowEnemyFinisher());
        StartCoroutine(RotateCamera());
    }

    private IEnumerator RotateCamera()
    {
        // Lerp to the end rotation
        float lerpTime = 0f;

        while (lerpTime < lerpToEndDuration)
        {
            lerpTime += Time.deltaTime;
            playerCamera.rotation = Quaternion.Slerp(startRotation, endRotation, lerpTime / lerpToEndDuration);
            yield return null; // Wait for the next frame
        }

        // Set the camera to the exact end rotation
        playerCamera.rotation = endRotation;

        // Wait for the stay duration
        yield return new WaitForSeconds(stayDuration);

        // Lerp back to the start rotation
        lerpTime = 0f;

        while (lerpTime < lerpToStartDuration)
        {
            lerpTime += Time.deltaTime;
            playerCamera.rotation = Quaternion.Slerp(endRotation, startRotation, lerpTime / lerpToStartDuration);
            yield return null; // Wait for the next frame
        }

        // Set the camera to the exact start rotation
        playerCamera.rotation = startRotation;
    }


private IEnumerator ShowEnemyFinisher()
    {

        // Ensure the final rotation is exactly the end rotation at the end of the coroutine
        playerCamera.transform.localRotation = startRotation;
        rbInputManager.ControlsEnabled(false);
        ghoulFinishedEnemy.SetActive(true);
        yield return new WaitForSeconds(finisherTime);
        ParticleSystem ps = ghoulFinishedEnemy.GetComponent<ParticleSystem>();
        ps.Play();
        SkinnedMeshRenderer smr = ghoulFinishedEnemy.GetComponent<SkinnedMeshRenderer>();
        smr.enabled = false;
        yield return new WaitForSeconds(0.5f);
        ps.Stop();
        smr.enabled = true;
        ghoulFinishedEnemy.SetActive(false);
        rbInputManager.ControlsEnabled(true);
        Die();
    }

   


// Die and destroy the enemy
private void Die()
    {
        // Handle enemy death logic here
        audioManager.PlayEnemyDieAudioClip();
        //Destroy(thisgameObject);
        Debug.Log("enemy died");
    }
}
