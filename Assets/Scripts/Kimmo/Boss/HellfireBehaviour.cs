using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellfireBehaviour : MonoBehaviour
{
    [SerializeField] int damage;
    PlayerHealthManager playerHealthManager;
    [SerializeField] Vector3 startingPosition;

    private void Awake()
    {
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        startingPosition = transform.position;
        gameObject.SetActive(false);
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
            ResetObject();
        }
    }

    private void ResetObject()
    {
        transform.position = startingPosition;
        gameObject.SetActive(false);
    }
}
