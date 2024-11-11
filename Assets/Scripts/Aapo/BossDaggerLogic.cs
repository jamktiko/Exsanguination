using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDaggerLogic : MonoBehaviour
{

    [SerializeField] int damage;
    [SerializeField] float projectileSpeed;
    private PlayerCombat playerCombat;
    private PlayerHealthManager playerHealthManager;
    private Transform player;             // Reference to the player's position
    private Rigidbody rb;
    public bool isReflected;

    GameObject daggerStartingPoint;
    public bool isThrown;

    private void Awake()
    {
        playerCombat = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<PlayerCombat>();
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        daggerStartingPoint = GameObject.Find("DaggerStartingPoint");
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.SetParent(null);
        rb.velocity = Vector3.zero;
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
        transform.rotation = targetRotation;
        rb.velocity = transform.forward * projectileSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (playerCombat.isBlocking)
            {
                rb.isKinematic = true;
                Vector3 directionToEnemy = (transform.position - player.position).normalized;
                rb.isKinematic = false;
                rb.velocity = directionToEnemy * projectileSpeed;  // Set the velocity directly
                isReflected = true;
                Debug.Log("reflected");
            }
            else
            {
                playerHealthManager.UpdatePlayerHealth(-damage);
                Debug.Log("Hit player");
                ResetValues();
            }

        }

        else if (isReflected && collision.collider.CompareTag("Boss"))
        {
            EnemyHealthScript enemyHealthScript = collision.collider.GetComponent<EnemyHealthScript>();

            if (enemyHealthScript != null)
            {
                enemyHealthScript.ChangeEnemyHealth(damage);
                Debug.Log("Reflected back to enemy and dealt damage");
            }
            else
            {
                Debug.LogWarning("EnemyHealthScript not found on " + collision.collider.name);
            }
        }

        else

        {
            ResetValues();
        }
    }

    private void ResetValues()
    {
        //transform.localPosition = Vector3.zero;
        //gameObject.SetActive(false);


        transform.SetParent(daggerStartingPoint.transform);
        transform.position = daggerStartingPoint.transform.position;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        gameObject.SetActive(false);
        isReflected = false;

    }
}
