using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDamage : MonoBehaviour
{
    [SerializeField] int damage;
    PlayerHealthManager playerHealthManager;
    [SerializeField] bool canDamage;
    public bool isActive;
    [SerializeField] GameObject gasObject;

    private void Awake()
    {
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
    }

    private void Start()
    {
        gasObject.SetActive(false);
    }

    private void Update()
    {
        if (isActive)
        {
            gasObject.SetActive(true);
        }

        if (canDamage)
        {
            Damage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isActive)
        {
            canDamage = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && isActive)
        {
            canDamage = false;
        }
    }

    private void Damage()
    {
        canDamage = false;
        playerHealthManager.UpdatePlayerHealth(-damage);
        StartCoroutine(WaitBeforeNextDamage());
    }

    IEnumerator WaitBeforeNextDamage()
    {
        yield return new WaitForSeconds(1);
        canDamage = true;
    }
}
