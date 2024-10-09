using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class EnemyGroup // Information of each enemy group
{
    [Header("Enemy name must be 'EnemyGhoul' without numbers")]
    [SerializeField] GameObject[] enemies;

    public GameObject spawnTriggerPoint;
    public int groupNumber;

    public void ActivateEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }
    }

    public void DeActivateEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }
    }
}

public class EnemySpawnerSystem : MonoBehaviour // Manages the enemy groups
{
    public EnemyGroup[] enemyGroup;
    private EnemyGroup currentEnemyGroup;

    private void Awake()
    {
        foreach (EnemyGroup group in enemyGroup)
        {
            group.DeActivateEnemies();
            group.spawnTriggerPoint.GetComponent<SpawnTriggerDetector>().SetGroupIndex(group.groupNumber);
        }
    }
}
