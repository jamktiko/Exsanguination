using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerSystem : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    private void Awake()
    {
        foreach (var enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var enemy in enemies)
            {
                enemy.SetActive(true);
            }
        }
    }
}
