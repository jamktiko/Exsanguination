using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class Boss : MonoBehaviour
{
    public BossStateManager bossStateManager { get; set; }
    public IdleState idleState { get; set; }
    public DashState dashState { get; set; }
    public StunState stunState { get; set; }
    public ChargeState chargeState { get; set; }
    public MeleeAttackState meleeAttackState { get; set; }
    public SpecialAttackState specialAttackState { get; set; }
    public DeathState deathState { get; set; }

    [Header("Movement")]
    public Animator bossAnimator;
    [SerializeField] Collider bossDamageCollider;
    [SerializeField] float moveSpeed;
    [SerializeField] float stalkSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform[] waypoints;
    public Transform bossTransform;
    public Transform playerTransform;
    public Vector3 targetPosition;
    Vector3 startPosition;
    float minDistance = 5f; // Minimum distance from player to exclude a waypoint
    public bool isInMeleeRange;
    [SerializeField] float meleeAttackSpeed;
    [SerializeField] float meleeAttackDistance;

    [Header("Vent")]
    [SerializeField] List<GameObject> vents;

    [Header("Stun")]
    public bool isStunned;
    public float stunDuration;
    public float idleDuration;

    [Header("SpecialAttacks")]
    System.Action[] specialAttacks;
    System.Action currentSpecialAttack;
    string[] animationTriggers;
    float[] castingTimes;
    public bool isCastingSpecialAttack;

    [Header("SpikeGrowth")]
    [SerializeField] GameObject spikeTrap;
    [SerializeField] Vector3 spikeTrapStartingPosition;

    [Header("Firewall")]
    FirewallBehaviour fireWallBehaviour;
    [SerializeField] GameObject firewall;
    [SerializeField] GameObject firewallPoint;

    [Header("Pirouette")]
    [SerializeField] Transform spinSpoint;
    public float spinDuration;
    [SerializeField] float elapsedSpinTime;
    [SerializeField] int daggerAmount;
    bool isSpinning;
    bool canThrow;
    public GameObject[] daggers;
    GameObject currentDagger;
    [SerializeField] int daggerIndex;
    [SerializeField] float timeBetweenDaggers;

    [Header("Hellfire")]
    [SerializeField] GameObject hellfire;
    [SerializeField] GameObject hellfirePoint;

    [Header("Audio")]
    [SerializeField] AudioClip[] bossCasts;
    [SerializeField] AudioSource castSpecialAttack;
    [SerializeField] AudioSource bossDashAudioSource;
    [SerializeField] AudioSource bossDiesAudioSource;
    AudioManager audioManager;

    [Header("Death")]
    public PauseScript pauseScript;
    public PlayerStats playerStats;
    PlayerHealthManager playerHealthManager;

    [Header("SFX")]
    public AudioMixer SFXMixer;

    private void Awake()
    {
        bossStateManager = new BossStateManager();

        idleState = new IdleState(this, bossStateManager);
        dashState = new DashState(this, bossStateManager);
        stunState = new StunState(this, bossStateManager);
        chargeState = new ChargeState(this, bossStateManager);
        meleeAttackState = new MeleeAttackState(this, bossStateManager);
        specialAttackState = new SpecialAttackState(this, bossStateManager);
        deathState = new DeathState(this, bossStateManager);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        pauseScript = GameObject.Find("PauseManager").GetComponent<PauseScript>();
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
    }

    private void Start()
    {
        bossStateManager.states = new BossAbstractState[] { chargeState, meleeAttackState, stunState, dashState, idleState, dashState, specialAttackState };
        bossStateManager.Initialize(bossStateManager.states[0]);

        specialAttacks = new System.Action[] { CastSpikeGrowth, CastPirouette, CastFirewall, CastHellfire };
        animationTriggers = new string[] { "spikeGrowth", "pirouette", "firewall", "hellfire" };
        castingTimes = new float[] { 2f, 1.5f, 2f, 1f };

        //specialAttacks = new System.Action[] { CastSpikeGrowth, CastSpikeGrowth, CastSpikeGrowth, CastSpikeGrowth };
        //animationTriggers = new string[] { "spikeGrowth", "spikeGrowth", "spikeGrowth", "spikeGrowth" };
        //castingTimes = new float[] { 2f, 2f, 2f, 2f };

        //timeBetweenDaggers = spinDuration / daggerAmount;
        //timeBetweenDaggers = 0.01f;
    }

    private void Update()
    {
        bossStateManager.currentBossState.FrameUpdate();

        if (isSpinning)
        {
            PirouetteSpin();
            ThrowDagger();
        }
    }

    private void FixedUpdate()
    {
        bossStateManager.currentBossState.PhysicsUpdate();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isInMeleeRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInMeleeRange = false;
        }
    }
    
    public void PickAndActivateVents(int ventsNumber)
    {
        List<GameObject> chosenVents = new List<GameObject>();

        // Randomly pick vents
        for (int i = 0; i < ventsNumber && vents.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, vents.Count);
            GameObject selectedVent = vents[randomIndex];
            chosenVents.Add(selectedVent);

            // Remove from the original list to avoid picking the same vent again
            vents.RemoveAt(randomIndex);
        }

        // Activate the method on each chosen vent
        foreach (GameObject vent in chosenVents)
        {
            vent.GetComponent<VentActivation>().SetGasActive();
        }
    }

    public void DeactivateBossCollider()
    {
        bossDamageCollider.enabled = false;
    }

    public void ActivateBossCollider()
    {
        bossDamageCollider.enabled = true;
    }

    public void ChooseWaypoint()
    {
        List<Transform> availableWaypoints = new List<Transform>();

        foreach (Transform waypoint in waypoints)
        {
            if (Vector3.Distance(waypoint.position, playerTransform.position) >= minDistance)
            {
                availableWaypoints.Add(waypoint);
            }
        }

        targetPosition = availableWaypoints[Random.Range(0, availableWaypoints.Count)].position;
    }

    public void RotateTowardsTarget()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            bossTransform.rotation = Quaternion.Slerp(bossTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void MoveTowardsTarget()
    {
        var actualTargetPosition = new Vector3(targetPosition.x, bossTransform.position.y, targetPosition.z);
        bossTransform.position = Vector3.MoveTowards(bossTransform.position, actualTargetPosition,
            moveSpeed * Time.deltaTime);
    }

    public void StartOfDash()
    {
        audioManager.PlayBossDashDamageClip(bossDashAudioSource);
    }

    public void MoveSideways()
    {
        AnimatorStateInfo stateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Stalking Right"))
        {
            bossTransform.position += bossTransform.right * stalkSpeed * Time.deltaTime;
        }   

        else if (stateInfo.IsName("Stalking Left"))
        {
            bossTransform.position -= bossTransform.right * stalkSpeed * Time.deltaTime;
        }
    }

    public void MeleeAttackMove()
    {
        if (Vector3.Distance(startPosition, bossTransform.position) < meleeAttackDistance)
        {
            bossTransform.Translate(Vector3.forward * meleeAttackSpeed * Time.deltaTime);
        }
    }

    public void SetStartPosition()
    {
        startPosition = bossTransform.position;
    }

    public void RandomizeSpecialAttack()
    {
        int randomIndex;
        randomIndex = Random.Range(0, specialAttacks.Length);
        currentSpecialAttack = specialAttacks[randomIndex];
        bossAnimator.SetTrigger(animationTriggers[randomIndex]);
        isCastingSpecialAttack = true;
        AudioClip castClip = castSpecialAttack.clip = bossCasts[randomIndex];
        castSpecialAttack.PlayOneShot(castClip);
        StartCoroutine(CastTimer(castingTimes[randomIndex]));
    }

    IEnumerator CastTimer(float castTime)
    {
        yield return new WaitForSeconds(castTime);
        currentSpecialAttack();
        //isCastingSpecialAttack = false;
    }

    private void CastSpikeGrowth()
    {
        Debug.Log("Boss special attack is: SPIKE GROWTH!");
        
        spikeTrap.transform.position = new Vector3(playerTransform.position.x, bossTransform.position.y, playerTransform.position.z);
        isCastingSpecialAttack = false;
        StartCoroutine(SpikeTrapTimer());
    }

    IEnumerator SpikeTrapTimer()
    {
        yield return new WaitForSeconds(10);
        spikeTrap.transform.position = spikeTrapStartingPosition;
    }

    private void CastPirouette()
    {
        Debug.Log("Boss special attack is: PIROUETTE!");
        
        isSpinning = true;
        canThrow = true;
    }

    private void PirouetteSpin()
    {
        elapsedSpinTime += Time.deltaTime;
        float rotationAngle = (elapsedSpinTime / spinDuration) * 360f;
        spinSpoint.eulerAngles = new Vector3(0, rotationAngle % 360, 0);

        if (elapsedSpinTime >= spinDuration)
        {
            elapsedSpinTime = 0f;
            isSpinning = false;
            isCastingSpecialAttack = false;
        }
    }

    private void ThrowDagger()
    {
        if (!canThrow) return;

        currentDagger = daggers[daggerIndex];
        currentDagger.SetActive(true);

        if (daggerIndex < daggers.Length - 1)
        {
            daggerIndex++;
        }
        else
        {
            daggerIndex = 0;
        }

        StartCoroutine(WaitBetweenThrows());
    }

    IEnumerator WaitBetweenThrows()
    {
        canThrow = false;
        yield return new WaitForSeconds(timeBetweenDaggers);
        canThrow = true;
    }

    private void CastFirewall()
    {
        Debug.Log("Boss' special attack is: FIREWALL!");
       
        firewall.transform.position = new Vector3(firewallPoint.transform.position.x, bossTransform.position.y, firewallPoint.transform.position.z);

        Vector3 bossRotation = bossTransform.eulerAngles;
        firewall.transform.eulerAngles = new Vector3(
            firewall.transform.eulerAngles.x, bossRotation.y, firewall.transform.eulerAngles.z);
        firewall.SetActive(true);
        isCastingSpecialAttack = false;
    }

    private void CastHellfire()
    {
        Debug.Log("Boss' special attack is: HELLFIRE!");
       
        hellfire.transform.position = hellfirePoint.transform.position;
        hellfire.SetActive(true);
        hellfire.GetComponent<ParticleSystem>().Play();
        isCastingSpecialAttack = false;
    }

    public void StartDeathState()
    {
        bossStateManager.isDead = true;
        audioManager.PlayBossDiesClip(bossDiesAudioSource);
        playerHealthManager.canTakeDamage = false;
        deathState.EnterState();
    }

    public void ShowVictoryScreen()
    {
        StartCoroutine(WaitBeforeVictoryScreen());
    }

    public IEnumerator WaitBeforeVictoryScreen()
    {
        yield return new WaitForSeconds(5);
        pauseScript.PauseGame();
        pauseScript.ShowVictoryScreen();
        playerStats.StopTimer();
        playerStats.hasWon = true;
        if (SFXMixer != null)
        {
            SFXMixer.SetFloat("SFXVolume", -999f); // 0 dB is represented by 0 in AudioMixer
        }
    }

}
