using KnowerCoder.BloodFX;
using UnityEngine;
using System.Collections;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    public int health = 100;
    AudioManager audioManager;
    [SerializeField] AudioSource enemyTakeDamageAudioSource;
    [SerializeField] AudioSource bossTakeDamageAudioSource;
    private EnemyFinisher stuckEnemyFinisher;
    private EnemyDeathScript enemyDeathScript;
    [SerializeField] private BloodFXController bloodController;
    [SerializeField] private ParticleSystem bloodSplatterParticle;
    [SerializeField] private StakeLogic stakeLogic;

    [SerializeField] Boss boss;
    [SerializeField] bool isBoss;
    public bool hasBeenDamaged;
    int previousHealth;
    PauseScript pauseScript;
    PlayerStats playerStats;

    [SerializeField] Renderer modelRenderer;
    Color damagedEffectColor = new Color(0.5f, 0.25f, 0.25f);
    private Color originalColor = Color.white; // Assuming the original color is white
    private bool isTransitioning = false; // Prevent overlapping transitions
    private MusicManager musicManager;

    private void Awake()
    {
        stuckEnemyFinisher = GameObject.Find("PlayerModel").GetComponent<EnemyFinisher>();
        enemyDeathScript = GetComponentInChildren<EnemyDeathScript>();
        bloodController = GetComponentInChildren<BloodFXController>();
        bloodSplatterParticle = GetComponentInChildren<ParticleSystem>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        stakeLogic = GameObject.FindGameObjectWithTag("Stake").GetComponent<StakeLogic>();
        pauseScript = GameObject.Find("PauseManager").GetComponent<PauseScript>();
        playerStats = GameObject.FindWithTag("PlayerStats").GetComponent<PlayerStats>();

        if (isBoss)
        {
            boss = GetComponent<Boss>();
        }
        else
        {
            modelRenderer = GetComponentInChildren<Renderer>();

            if (modelRenderer != null)
            {
                originalColor = modelRenderer.material.GetColor("_BaseColor");
            }
        }
    }

    private void Start()
    {
        health = maxHealth;
        previousHealth = maxHealth;
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    public void ChangeEnemyHealth(int changeAmount)
    {
        audioManager.PlayEnemyTakeDamageAudioClip(enemyTakeDamageAudioSource);
        bloodController.Shoot();
        bloodSplatterParticle.Play(true);
        health = Mathf.Clamp(health - changeAmount, 0, maxHealth);
        Debug.Log("Enemy health = " + health);

        if (health <= 15 && !isBoss && !isTransitioning)
        {
            StartCoroutine(SmoothlyChangeColor(damagedEffectColor, 0.1f));
        }
        if (health <= 0 && !isBoss)
        {
            Debug.Log("Enemy health is zero");
            EnemyDie();
        }

        if (isBoss)
        {
            audioManager.PlayBossTakeDamageClip(bossTakeDamageAudioSource);
            BossHealthEffects();
        }
    }


    private IEnumerator SmoothlyChangeColor(Color targetColor, float duration)
    {
        isTransitioning = true;
        float elapsedTime = 0f;

        Color startingColor = modelRenderer.material.GetColor("_BaseColor");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate between the starting color and the target color
            modelRenderer.material.SetColor("_BaseColor", Color.Lerp(startingColor, targetColor, t));
            yield return null;
        }

        // Ensure the color is set to the exact target at the end
        modelRenderer.material.SetColor("_BaseColor", targetColor);
        isTransitioning = false;
    }

    public void FinishEnemy()
    {
        stuckEnemyFinisher.Finish();
        gameObject.SetActive(false);
    }

    public int GetEnemyHealth()
    {
        return health;
    }
    public int GetEnemyMaxHealth()
    {
        return maxHealth;
    }

    public void EnemyDie()
    {
        enemyDeathScript.EnemyDie();
        if (stakeLogic.stuckEnemyHealth != null)
        {
            if (gameObject == stakeLogic.stuckEnemyHealth.gameObject)
                stakeLogic.ResetConnectionToEnemy();
        }
        gameObject.SetActive(false);
    }

    private void BossHealthEffects()
    {
        if (previousHealth > 150 && health <= 150)
        {
            musicManager.ChangeBossMusicVariation(1);
            boss.idleDuration = 3;
            boss.PickAndActivateVents(3);
        }

        else if (previousHealth > 100 && health <= 100)
        {
            musicManager.ChangeBossMusicVariation(2);
            boss.idleDuration = 1;
            boss.PickAndActivateVents(4);
        }

        else if (previousHealth > 50 && health <= 50)
        {
            musicManager.ChangeBossMusicVariation(3);
            boss.idleDuration = 0.1f;
            boss.PickAndActivateVents(7);
        }

        else if (previousHealth > 0 && health <= 0)
        {
            musicManager.ChangeBossMusicVariation(4);
            boss.StartDeathState();
        }

        previousHealth = health;
    }
}