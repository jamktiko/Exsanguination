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
    [SerializeField] AudioSource swordEquipAudioSource;
    [SerializeField] AudioSource swordSwingAudioSource;
    [SerializeField] AudioSource parryAudioSource;
    [SerializeField] AudioSource grapplingHookReadyAudioSource;
    [SerializeField] AudioSource grapplingHookShootAudioSource;
    [SerializeField] AudioSource grapplingHookHitAudioSource;
    [SerializeField] AudioSource stakeThrowAudioSource;
    [SerializeField] AudioSource stakeHitAudioSource;
    [SerializeField] AudioSource stakePickUpAudioSource;
    [SerializeField] AudioSource stakeFinisherAudioSource;
    [SerializeField] AudioSource playerTakeDamageAudioSource;
    [SerializeField] AudioSource playerDieAudioSource;
    [SerializeField] AudioSource playerHealAudioSource;
    [SerializeField] AudioSource playerFallAudioSource;
    [SerializeField] AudioSource ventAudioSource;


    [Header("SFX clips")]

    [Header("Player")]

    [SerializeField] AudioClip[] playerFootstepsAudioClips;
    [SerializeField] AudioClip playerJumpAudioClip;
    [SerializeField] AudioClip playerLandAudioClip;
    [SerializeField] AudioClip dashAudioClip;
    [SerializeField] AudioClip slideAudioClip;
    [SerializeField] AudioClip[] swordEquipAudioClips;
    [SerializeField] AudioClip[] swordSwingCombo1AudioClips;
    [SerializeField] AudioClip[] swordSwingCombo2AudioClips;
    [SerializeField] AudioClip[] swordSwingCombo3AudioClips;
    [SerializeField] AudioClip parryAudioClip;
    [SerializeField] AudioClip grapplingHookReadyAudioClip;
    [SerializeField] AudioClip grapplingHookShootAudioClip;
    [SerializeField] AudioClip grapplingHookHitAudioClip;
    [SerializeField] AudioClip stakeThrowAudioClip;
    [SerializeField] AudioClip stakeHitAudioClip;
    [SerializeField] AudioClip stakePickUpAudioClip;
    [SerializeField] AudioClip stakeFinisherAudioClip;
    [SerializeField] AudioClip playerTakeDamageAudioClip;
    [SerializeField] AudioClip playerDieAudioClip;
    [SerializeField] AudioClip playerHealAudioClip;
    [SerializeField] AudioClip playerFallAudioClip;

    [Header("Ghoul")]

    [SerializeField] AudioClip enemyAlertAudioClip;
    [SerializeField] AudioClip[] enemyFootstepsAudioClips;
    [SerializeField] AudioClip[] enemyMeleeAudioClips;
    [SerializeField] AudioClip enemyTakeDamageAudioClip;
    [SerializeField] AudioClip enemyDieAudioClip;

    [Header("Bat")]
    [SerializeField] AudioClip batAttackAudioClip;
    [SerializeField] AudioClip batDieAudioClip;
    [SerializeField] AudioClip batAlertAudioClip;

    [Header("Boss")]
    [SerializeField] AudioClip[] bossTakeDamageAudioClips;
    [SerializeField] AudioClip bossDashAudioClip;

    [Header("Vent")]
    [SerializeField] AudioClip ventAudioClip;

    private void Awake()
    {
        playerFootstepsAudioSource = GameObject.Find("PlayerFootstepsAudioSource").GetComponent<AudioSource>();
        playerJumpAudioSource = GameObject.Find("PlayerJumpAudioSource").GetComponent<AudioSource>();
        playerLandAudioSource = GameObject.Find("PlayerLandAudioSource").GetComponent<AudioSource>();
        dashAudioSource = GameObject.Find("DashAudioSource").GetComponent<AudioSource>();
        slideAudioSource = GameObject.Find("SlideAudioSource").GetComponent<AudioSource>();
        swordEquipAudioSource = GameObject.Find("SwordEquipAudioSource").GetComponent<AudioSource>();
        swordSwingAudioSource = GameObject.Find("SwordSwingAudioSource").GetComponent<AudioSource>();
        parryAudioSource = GameObject.Find("ParryAudioSource").GetComponent<AudioSource>();
        grapplingHookReadyAudioSource = GameObject.Find("GrapplingHookReadyAudioSource").GetComponent<AudioSource>();
        grapplingHookShootAudioSource = GameObject.Find("GrapplingHookShootAudioSource").GetComponent<AudioSource>();
        grapplingHookHitAudioSource = GameObject.Find("GrapplingHookHitAudioSource").GetComponent<AudioSource>();
        stakeThrowAudioSource = GameObject.Find("StakeThrowAudioSource").GetComponent<AudioSource>();
        stakeHitAudioSource = GameObject.Find("StakeHitAudioSource").GetComponent<AudioSource>();
        stakePickUpAudioSource = GameObject.Find("StakePickUpAudioSource").GetComponent<AudioSource>();
        stakeFinisherAudioSource = GameObject.Find("StakeFinisherAudioSource").GetComponent<AudioSource>();
        playerTakeDamageAudioSource = GameObject.Find("PlayerTakeDamageAudioSource").GetComponent<AudioSource>();
        playerDieAudioSource = GameObject.Find("PlayerDieAudioSource").GetComponent<AudioSource>();
        playerHealAudioSource = GameObject.Find("PlayerHealAudioSource").GetComponent<AudioSource>();
        playerFallAudioSource = GameObject.Find("PlayerFallAudioSource").GetComponent<AudioSource>();
    }

    // Player SFX methods

    // Footsteps
    public void PlayPlayerFootstepsAudioClips()
    {
        AudioClip clip = playerFootstepsAudioClips[Random.Range(0, playerFootstepsAudioClips.Length)];
        playerFootstepsAudioSource.clip = clip;
        playerFootstepsAudioSource.PlayOneShot(clip);
    }

    // Jump
    public void PlayPlayerJumpAudioClip()
    {
        playerJumpAudioSource.clip = playerJumpAudioClip;
        playerJumpAudioSource.PlayOneShot(playerJumpAudioClip);
    }

    // Land
    public void PlayPlayerLandAudioClip()
    {
        playerLandAudioSource.clip = playerLandAudioClip;
        playerLandAudioSource.PlayOneShot(playerLandAudioClip);
    }

    // Dash
    public void PlayDashAudioClip()
    {
        dashAudioSource.clip = dashAudioClip;
        dashAudioSource.PlayOneShot(dashAudioClip);
    }

    // Slide
    public void PlaySlideAudioClip()
    {
        slideAudioSource.clip = slideAudioClip;
        slideAudioSource.PlayOneShot(slideAudioClip);
    }

    // Sword equip
    public void PlaySwordEquipClips()
    {
        AudioClip clip = swordEquipAudioClips[Random.Range(0, swordEquipAudioClips.Length)];
        swordEquipAudioSource.clip = clip;
        swordEquipAudioSource.PlayOneShot(clip);
    }

    // Sword swing combo 1
    public void PlaySwordSwingClips1()
    {
        float pitchValue = Random.Range(0.95f, 1.05f);
        swordSwingAudioSource.pitch = pitchValue;
        AudioClip clip = swordSwingCombo1AudioClips[Random.Range(0, swordSwingCombo1AudioClips.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }

    // Sword swing combo 2
    public void PlaySwordSwingClips2()
    {
        float pitchValue = Random.Range(0.95f, 1.05f);
        swordSwingAudioSource.pitch = pitchValue;
        AudioClip clip = swordSwingCombo2AudioClips[Random.Range(0, swordSwingCombo2AudioClips.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }

    // Sword swing combo 3
    public void PlaySwordSwingClips3()
    {
        float pitchValue = Random.Range(0.95f, 1.05f);
        swordSwingAudioSource.pitch = pitchValue;
        AudioClip clip = swordSwingCombo3AudioClips[Random.Range(0, swordSwingCombo3AudioClips.Length)];
        swordSwingAudioSource.clip = clip;
        swordSwingAudioSource.PlayOneShot(clip);
    }

    // Parry
    public void PlayParryAudioClip()
    {
        parryAudioSource.clip = parryAudioClip;
        parryAudioSource.PlayOneShot(parryAudioClip);
    }

    // Grappling hook ready
    public void PlayGrapplingHookReadyAudioClip()
    {
        grapplingHookReadyAudioSource.clip = grapplingHookReadyAudioClip;
        grapplingHookReadyAudioSource.PlayOneShot(grapplingHookReadyAudioClip);
    }

    // Grappling hook shoot
    public void PlayGrapplingHookShootAudioClip()
    {
        grapplingHookShootAudioSource.clip = grapplingHookShootAudioClip;
        grapplingHookShootAudioSource.PlayOneShot(grapplingHookShootAudioClip);
    }

    // Grappling hook hit
    public void PlayGrapplingHookHitAudioClip()
    {
        grapplingHookHitAudioSource.clip = grapplingHookHitAudioClip;
        grapplingHookHitAudioSource.PlayOneShot(grapplingHookHitAudioClip);
    }

    // Stake throw
    public void PlayStakeThrowAudioClip()
    {
        stakeThrowAudioSource.clip = stakeThrowAudioClip;
        stakeThrowAudioSource.PlayOneShot(stakeThrowAudioClip);
    }

    // Stake hit
    public void PlayStakeHitAudioClip()
    {
        stakeHitAudioSource.clip = stakeHitAudioClip;
        stakeHitAudioSource.PlayOneShot(stakeHitAudioClip);
    }

    // Stake pick up
    public void PlayStakePickUpAudioClip()
    {
        stakePickUpAudioSource.clip = stakePickUpAudioClip;
        stakePickUpAudioSource.PlayOneShot(stakePickUpAudioClip);
    }

    // Stake finisher
    public void PlayStakeFinisherAudioClip()
    {
        stakeFinisherAudioSource.clip = stakeFinisherAudioClip;
        stakeFinisherAudioSource.PlayOneShot(stakeFinisherAudioClip);
    }

    // Player take damage
    public void PlayPlayerTakeDamageAudioClip()
    {
        playerTakeDamageAudioSource.clip = playerTakeDamageAudioClip;
        playerTakeDamageAudioSource.PlayOneShot(playerTakeDamageAudioClip);
    }

    // Player die
    public void PlayPlayerDieAudioClip()
    {
        playerDieAudioSource.clip = playerDieAudioClip;
        playerDieAudioSource.PlayOneShot(playerDieAudioClip);
    }

    // Player heal
    public void PlayPlayerHealAudioClip()
    {
        playerHealAudioSource.clip = playerHealAudioClip;
        playerHealAudioSource.PlayOneShot(playerHealAudioClip);
    }

    // Player fall
    public void PlayPlayerFallAudioClip()
    {
        playerFallAudioSource.clip = playerFallAudioClip;
        playerFallAudioSource.PlayOneShot(playerFallAudioClip);
    }

    // Enemy SFX methods

    // Enemy alert
    public void PlayEnemyAlertAudioClip(AudioSource audioSource)
    {
        audioSource.clip = enemyAlertAudioClip;
        audioSource.PlayOneShot(enemyAlertAudioClip);
    }

    // Enemy footsteps
    public void PlayEnemyFootstepsAudioClips(AudioSource audioSource)
    {
        AudioClip clip = enemyFootstepsAudioClips[Random.Range(0, enemyFootstepsAudioClips.Length)];
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    // Melee swing
    public void PlayEnemyMeleeAudioClips(AudioSource audioSource)
    {
        AudioClip clip = enemyMeleeAudioClips[Random.Range(0, enemyMeleeAudioClips.Length)];
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    //  Enemy take damage
    public void PlayEnemyTakeDamageAudioClip(AudioSource audioSource)
    {
        audioSource.clip = enemyTakeDamageAudioClip;
        audioSource.PlayOneShot(enemyTakeDamageAudioClip);
    }

    // Enemy die
    public void PlayEnemyDieAudioClip(AudioSource audioSource)
    {
        audioSource.clip = enemyDieAudioClip;
        audioSource.PlayOneShot(enemyDieAudioClip);
    }

    public void PlayEnemyBatAlertClip(AudioSource audioSource)
    {
        audioSource.clip = batAlertAudioClip;
        audioSource.PlayOneShot(batAlertAudioClip);

    }

    // Bat
    public void PlayEnemyBatAttackClip(AudioSource audioSource)
    {
        audioSource.clip = batAttackAudioClip;
        audioSource.PlayOneShot(batAttackAudioClip);
    }

    // Boss take damage voice
    public void PlayBossTakeDamageClip(AudioSource audioSource)
    {
        AudioClip clip = bossTakeDamageAudioClips[Random.Range(0, bossTakeDamageAudioClips.Length)];
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    // Boss dash voice
    public void PlayBossDashDamageClip(AudioSource audioSource)
    {
        audioSource.clip = bossDashAudioClip;
        audioSource.PlayOneShot(bossDashAudioClip);
    }

    // Vent
    public void PlayVentClip(AudioSource audioSource)
    {
        audioSource.clip = ventAudioClip;
        audioSource.PlayOneShot(ventAudioClip);
    }

}
