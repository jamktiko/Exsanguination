using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX sourcess")]
    [SerializeField] AudioSource swordSwingAudioSource;

    [Header("SFX clips")]
    [SerializeField] AudioClip[] swordSwingAudioClips1;
    [SerializeField] AudioClip[] swordSwingAudioClips2;
    [SerializeField] AudioClip[] swordSwingAudioClips3;

    

    public void PlaySwordSwingClips1()
    {
        AudioClip clip = swordSwingAudioClips1[Random.Range(0, swordSwingAudioClips1.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }

    public void PlaySwordSwingClips2()
    {
        AudioClip clip = swordSwingAudioClips1[Random.Range(0, swordSwingAudioClips2.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }

    public void PlaySwordSwingClips3()
    {
        AudioClip clip = swordSwingAudioClips1[Random.Range(0, swordSwingAudioClips3.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }
}
