using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDamage : MonoBehaviour
{
    [SerializeField] int damage;
    PlayerHealthManager playerHealthManager;
    bool canDamage;
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
            canDamage = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && isActive)
        {
            Damage();
        }
    }

    private void Damage()
    {
        if (!canDamage) return;

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
