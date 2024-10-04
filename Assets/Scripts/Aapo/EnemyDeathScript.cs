using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathScript : MonoBehaviour
{
    private AudioSource audioSource;
    private ParticleSystem particleSystem;
    private AudioManager audioManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        particleSystem.Stop();
    }

    public void EnemyDie()
    {
        transform.SetParent(null);
        audioManager.PlayEnemyDieAudioClip(audioSource);
        particleSystem.Play();
        StartCoroutine(DisableComponent());

    }

    private IEnumerator DisableComponent()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
