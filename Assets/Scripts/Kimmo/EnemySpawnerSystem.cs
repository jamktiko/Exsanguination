using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class EnemyGroup // Information and methods of individual enemy groups
{
    [SerializeField] GameObject[] enemies;

    public GameObject spawnTriggerPoint;
    public int groupNumber;

    public void ActivateEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
               EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                enemyAI.ActivateEnemy();
            }
        }
    }

    //public void DeActivateEnemies()
    //{
    //    foreach (GameObject enemy in enemies)
    //    {
    //        if (enemy != null)
    //        {
    //            enemy.SetActive(false);
    //        }
    //    }
    //}
}

public class EnemySpawnerSystem : MonoBehaviour // Manages all enemy groups
{
    public EnemyGroup[] enemyGroup;
    private EnemyGroup currentEnemyGroup;

    private void Awake()
    {
        foreach (EnemyGroup group in enemyGroup)
        {
            //group.DeActivateEnemies();
            group.spawnTriggerPoint.GetComponent<SpawnTriggerDetector>().SetGroupIndex(group.groupNumber);
        }
    }
}
