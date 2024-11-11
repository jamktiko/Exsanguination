using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellfireBehaviour : MonoBehaviour
{
    [SerializeField] int damage;
    PlayerHealthManager playerHealthManager;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] bool isDestryed;

    private void Awake()
    {
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        startingPosition = gameObject.transform.position;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isDestryed)
        {
            ResetObject();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Hellfire hit player");
            playerHealthManager.UpdatePlayerHealth(-damage);
            ResetObject();
        }
        else if (collision.gameObject.tag == "Wall")
        {
            isDestryed = true;
        }
    }

    private void ResetObject()
    {
        transform.position = startingPosition;
        gameObject.SetActive(false);
    }
}
