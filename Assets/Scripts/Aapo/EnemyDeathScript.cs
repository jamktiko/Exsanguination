using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathScript : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private GameObject particleSystem;
    private AudioManager audioManager;
    public bool isDead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        
    }

    public void EnemyDie()
    {
        isDead = true;
        transform.SetParent(null);
        audioManager.PlayEnemyDieAudioClip(audioSource);
        Instantiate(particleSystem, gameObject.transform);
        StartCoroutine(DisableComponent());

    }

    private IEnumerator DisableComponent()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
