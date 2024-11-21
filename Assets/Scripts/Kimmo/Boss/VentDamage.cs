using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDamage : MonoBehaviour
{
    [SerializeField] int damage;
    PlayerHealthManager playerHealthManager;
    [SerializeField] bool canDamage;
    bool isTouchingPlayer;
    [SerializeField] GameObject gasObject;

    private void Awake()
    {
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
    }

    private void Start()
    {
        gasObject.SetActive(false);
    }

    public void SetGasActive()
    {
        gasObject.SetActive(true);
        canDamage = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isTouchingPlayer = true;

            if (canDamage)
            {
                Damage();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTouchingPlayer = false;
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

        if (isTouchingPlayer)
        {
            Damage();
        }
    }
}
