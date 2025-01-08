using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellfireBehaviour : MonoBehaviour
{
    [SerializeField] int damage;
    PlayerHealthManager playerHealthManager;
    PlayerCombat playerCombat;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] ParticleSystem flameParticle;

    private void Awake()
    {
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        playerCombat = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<PlayerCombat>();
        startingPosition = transform.localPosition;
        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        flameParticle.Play(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ResetObject();

            if (!playerCombat.isBlocking)
            {
                playerHealthManager.UpdatePlayerHealth(-damage);
            }
        }
        if (other.tag == "Wall")
        {
            ResetObject();
        }
    }

    private void ResetObject()
    {
        Debug.Log("Hellfire is reset");
        flameParticle.Stop();
        transform.localPosition = startingPosition;
        gameObject.SetActive(false);
        gameObject.GetComponent<ParticleSystem>().Play();
    }
}
