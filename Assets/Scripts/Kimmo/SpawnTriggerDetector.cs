using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTriggerDetector : MonoBehaviour
{
    [SerializeField] int groupIndex;
    EnemySpawnerSystem enemySpawnerSystem;

    public void SetGroupIndex(int groupNumber)
    {
        groupIndex = groupNumber;
    }

    private void Awake()
    {
        enemySpawnerSystem = FindObjectOfType<EnemySpawnerSystem>();

        if (enemySpawnerSystem == null)
        {
            Debug.LogError("EnemySpawnerSystem not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemySpawnerSystem.enemyGroup[groupIndex].ActivateEnemies();
        }
    }
}   
