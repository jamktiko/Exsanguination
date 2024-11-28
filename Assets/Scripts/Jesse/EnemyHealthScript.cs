using KnowerCoder.BloodFX;
using UnityEngine;

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
        }
    }

    private void Start()
    {
        health = maxHealth;
        previousHealth = maxHealth;
    }

    public void ChangeEnemyHealth(int changeAmount)
    {
        audioManager.PlayEnemyTakeDamageAudioClip(enemyTakeDamageAudioSource);
        bloodController.Shoot();
        bloodSplatterParticle.Play(true);
        health = Mathf.Clamp(health - changeAmount, 0, maxHealth);
        Debug.Log("Enemy health = " + health);

        if (health <= 15 && !isBoss)
        {
            modelRenderer.material.SetColor("_BaseColor", damagedEffectColor);
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
            boss.idleDuration = 3;
            boss.PickAndActivateVents(3);
        }

        else if (previousHealth > 100 && health <= 100)
        {
            boss.idleDuration = 1;
            boss.PickAndActivateVents(4);
        }

        else if (previousHealth > 50 && health <= 50)
        {
            boss.idleDuration = 0.1f;
            boss.PickAndActivateVents(7);
        }

        else if (previousHealth > 0 && health <= 0)
        {
            pauseScript.PauseGame();
            pauseScript.ShowVictoryScreen();
            playerStats.StopTimer();
        }

        previousHealth = health;
    }
}
