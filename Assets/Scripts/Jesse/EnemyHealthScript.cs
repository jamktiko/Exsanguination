using KnowerCoder.BloodFX;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;
    AudioManager audioManager;
    [SerializeField] AudioSource enemyTakeDamageAudioSource;
    private EnemyFinisher stuckEnemyFinisher;
    private EnemyDeathScript enemyDeathScript;
    [SerializeField] private BloodFXController bloodController;
    [SerializeField] private ParticleSystem bloodSplatterParticle;

    private void Awake()
    {
        stuckEnemyFinisher = GameObject.Find("PlayerModel").GetComponent<EnemyFinisher>();
        enemyDeathScript = GetComponentInChildren<EnemyDeathScript>();
        bloodController = GetComponentInChildren<BloodFXController>();
        bloodSplatterParticle = GetComponentInChildren<ParticleSystem>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();            
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void ChangeEnemyHealth(int changeAmount)
    {
        audioManager.PlayEnemyTakeDamageAudioClip(enemyTakeDamageAudioSource);
        bloodController.Shoot();
        bloodSplatterParticle.Play(true);
        health = Mathf.Clamp(health - changeAmount, 0, maxHealth);
        Debug.Log("Enemy health = " + health);
        if (health <= 0)
        {
            Debug.Log("Enemy health is zero");
            EnemyDie();
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
        gameObject.SetActive(false);
    }
}
