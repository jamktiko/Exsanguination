using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX sourcess")]
    [SerializeField] AudioSource playerFootstepsAudioSource;
    [SerializeField] AudioSource playerJumpAudioSource;
    [SerializeField] AudioSource playerLandAudioSource;
    [SerializeField] AudioSource dashAudioSource;
    [SerializeField] AudioSource slideAudioSource;
    [SerializeField] AudioSource grapplingHookReadyAudioSource;
    [SerializeField] AudioSource grapplingHookShootAudioSource;
    [SerializeField] AudioSource grapplingHookHitAudioSource;
    [SerializeField] AudioSource swordEquipAudioSource;
    [SerializeField] AudioSource swordSwingAudioSource;
    [SerializeField] AudioSource parryAudioSource;
    [SerializeField] AudioSource playerDamageAudioSource;
    [SerializeField] AudioSource playerDieAudioSource;
    [SerializeField] AudioSource enemyMeleeAudioSource;
    [SerializeField] AudioSource enemyFootstepsAudioSource;

    [Header("SFX clips")]
    [SerializeField] AudioClip[] playerFootstepsAudioClips; 
    [SerializeField] AudioClip playerJumpAudioClip;
    [SerializeField] AudioClip playerLandAudioClip;
    [SerializeField] AudioClip dashAudioClip;
    [SerializeField] AudioClip slideAudioClip;
    [SerializeField] AudioClip grapplingHookReadyAudioClip;
    [SerializeField] AudioClip grapplingHookShootAudioClip;
    [SerializeField] AudioClip grapplingHookHitAudioClip;
    [SerializeField] AudioClip[] swordEquipAudioClips;
    [SerializeField] AudioClip[] swordSwingCombo1AudioClips;
    [SerializeField] AudioClip[] swordSwingCombo2AudioClips;
    [SerializeField] AudioClip[] swordSwingCombo3AudioClips;
    [SerializeField] AudioClip parryAudioClip;
    [SerializeField] AudioClip playerDamageAudioClip;
    [SerializeField] AudioClip playerDieAudioClip;
    [SerializeField] AudioClip[] enemyFootstepsAudioClips;
    [SerializeField] AudioClip[] enemyMeleeAudioClips;

    /// Player SFX methods/
    public void PlayPlayerFootstepsAudioClips()
    {
        AudioClip clip = playerFootstepsAudioClips[Random.Range(0, playerFootstepsAudioClips.Length)];
        playerFootstepsAudioSource.clip = clip;
        playerFootstepsAudioSource.PlayOneShot(clip);
    }

    public void PlayPlayerJumpAudioClip()
    {
        playerJumpAudioSource.clip = playerJumpAudioClip;
        playerJumpAudioSource.PlayOneShot(playerJumpAudioClip);
    }

    public void PlayPlayerLandAudioClip()
    {
        playerLandAudioSource.clip = playerLandAudioClip;
        playerLandAudioSource.PlayOneShot(playerLandAudioClip);
    }

    public void PlayDashAudioClip()
    {
        dashAudioSource.clip = dashAudioClip;
        dashAudioSource.PlayOneShot(dashAudioClip);
    }

    public void PlaySlideAudioClip()
    {
        slideAudioSource.clip = slideAudioClip;
        slideAudioSource.PlayOneShot(slideAudioClip);
    }

    public void PlayGrapplingHookReadyAudioClip()
    {
        grapplingHookReadyAudioSource.clip = grapplingHookReadyAudioClip;
        grapplingHookReadyAudioSource.PlayOneShot(grapplingHookReadyAudioClip);
    }

    public void PlayGrapplingHookShootAudioClip()
    {
        grapplingHookShootAudioSource.clip = grapplingHookShootAudioClip;
        grapplingHookShootAudioSource.PlayOneShot(grapplingHookShootAudioClip);
    }

    public void PlayGrapplingHookHitAudioClip()
    {
        grapplingHookHitAudioSource.clip = grapplingHookHitAudioClip;
        grapplingHookHitAudioSource.PlayOneShot(grapplingHookHitAudioClip);
    }

    public void PlaySwordEquipClips()
    {
        AudioClip clip = swordEquipAudioClips[Random.Range(0, swordEquipAudioClips.Length)];
        swordEquipAudioSource.clip = clip;
        swordEquipAudioSource.PlayOneShot(clip);
    }

    public void PlaySwordSwingClips1()
    {
        AudioClip clip = swordSwingCombo1AudioClips[Random.Range(0, swordSwingCombo1AudioClips.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }

    public void PlaySwordSwingClips2()
    {
        AudioClip clip = swordSwingCombo1AudioClips[Random.Range(0, swordSwingCombo2AudioClips.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }

    public void PlaySwordSwingClips3()
    {
        AudioClip clip = swordSwingCombo1AudioClips[Random.Range(0, swordSwingCombo3AudioClips.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }

    public void PlayParryAudioClip()
    {
        playerLandAudioSource.clip = parryAudioClip;
        playerLandAudioSource.PlayOneShot(parryAudioClip);
    }

    public void PlayPlayerDamageAudioClip()
    {
        playerDamageAudioSource.clip = playerDamageAudioClip;
        playerDamageAudioSource.PlayOneShot(playerDamageAudioClip);
    }

    public void PlayPlayerDieAudioClip()
    {
        playerDieAudioSource.clip = playerDieAudioClip;
        playerDieAudioSource.PlayOneShot(playerDieAudioClip);
    }

    // Enemy SFX methods
    public void PlayEnemyFootstepsAudioClips()
    {
        AudioClip clip = enemyFootstepsAudioClips[Random.Range(0, enemyFootstepsAudioClips.Length)];
        enemyFootstepsAudioSource.clip = clip;
        enemyFootstepsAudioSource.PlayOneShot(clip);
    }

    public void PlayEnemyMeleeAudioClips()
    {
        AudioClip clip = enemyMeleeAudioClips[Random.Range(0, enemyMeleeAudioClips.Length)];
        enemyMeleeAudioSource.clip = clip;
        enemyMeleeAudioSource.PlayOneShot(clip);
    }


}
