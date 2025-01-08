using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentActivation : MonoBehaviour
{
    Transform gasObject;
    Collider gasCollider;
    AudioManager audioManager;
    AudioSource gasAudioSource;

    private void Awake()
    {
        gasObject = transform.Find("Gas");
        gasCollider = GetComponent<Collider>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gasAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gasObject.gameObject.SetActive(false);
        gasCollider.enabled = false;
    }

    public void SetGasActive()
    {
        gasObject.gameObject.SetActive(true);
        gasCollider.enabled = true;
        audioManager.PlayVentClip(gasAudioSource);
    }
}
