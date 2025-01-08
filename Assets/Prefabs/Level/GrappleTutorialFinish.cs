using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTutorialFinish : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            animator.SetTrigger("Slide");
            audioSource.Play();
        }
    }
}
