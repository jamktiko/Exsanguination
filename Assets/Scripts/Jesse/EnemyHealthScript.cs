using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;
    [SerializeField] AudioManager audioManager;
    private GameObject finishedEnemyModel;


    private void Awake()
    {
        finishedEnemyModel = GameObject.Find(gameObject.name + "Finish");

    }

    private void Start()
    {
        health = maxHealth;
    }

    public void ChangeEnemyHealth(int changeAmount)
    {
        audioManager.PlayEnemyTakeDamageAudioClip();
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
        EnemyFinisher enemyFinisher = finishedEnemyModel.GetComponent<EnemyFinisher>();
        enemyFinisher.Finish();
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
        audioManager.PlayEnemyDieAudioClip();
        gameObject.SetActive(false);
        Debug.Log("enemy died");
    }
}
