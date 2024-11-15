using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellfireBehaviour : MonoBehaviour
{
    [SerializeField] int damage;
    PlayerHealthManager playerHealthManager;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] ParticleSystem flameParticle;

    private void Awake()
    {
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        startingPosition = transform.localPosition;
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
            playerHealthManager.UpdatePlayerHealth(-damage);
        }
        else if (other.includeLayers == 10)
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
    }
}
