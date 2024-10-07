using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup // Information of each enemy group
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject spawnTriggerPoint;
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
    [SerializeField] EnemyGroup[] enemyGroup;
    private EnemyGroup currentEnemyGroup;
    [SerializeField] GameObject[] spawnTriggerPoints;

    private void Awake()
    {
        foreach (EnemyGroup group in enemyGroup)
        {
            group.DeActivateEnemies();
        }
    }

    private void Update()
    {
        foreach (GameObject spawnTrigger in spawnTriggerPoints)
        {

        }
    }
}
