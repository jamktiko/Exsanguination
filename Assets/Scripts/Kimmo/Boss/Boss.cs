using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossStateManager bossStateManager { get; set; }
    public IdleState idleState { get; set; }
    public DashState dashState { get; set; }
    public StunState stunState { get; set; }
    public ChargeState chargeState { get; set; }
    public MeleeAttackState meleeAttackState { get; set; }
    public SpecialAttackState specialAttackState { get; set; }

    public Animator bossAnimator;
    [SerializeField] Collider bossCollider;
    [SerializeField] Collider swordCollider;
    [SerializeField] float moveSpeed;
    [SerializeField] float stalkSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float meleeAttackSpeed;
    [SerializeField] float meleeAttackDistance;
    Vector3 targetOffset;
    [SerializeField] Transform[] waypoints;
    public Transform bossTransform;
    public Transform playerTransform;
    public Vector3 targetPosition;
    Vector3 startPosition;
    float minDistance = 5f; // Minimum distance from player to exclude waypoint
    public bool isInMeleeRange;

    [SerializeField] List<GameObject> vents;
    GameObject currentVent;

    public bool isStunned;
    public float stunDuration;
    public float idleDuration;

    System.Action[] specialAttacks;
    System.Action currentSpecialAttack;
    string[] animationTriggers;
    float[] castingTimes;
    public bool isCastingSpecialAttack;

    [SerializeField] GameObject spikeTrap;
    [SerializeField] Vector3 spikeTrapStartingPosition;
    float spikeTrapDuration = 10f;

    FirewallBehaviour fireWallBehaviour;
    [SerializeField] GameObject firewall;
    [SerializeField] GameObject firewallPoint;

    [SerializeField] Transform spinSpoint;
    public float spinDuration;
    [SerializeField] float elapsedSpinTime;
    [SerializeField] int daggerAmount;
    bool isSpinning;
    bool canThrow;
    public GameObject[] daggers;
    GameObject currentDagger;
    int daggerIndex;

    [SerializeField] GameObject hellfire;
    [SerializeField] GameObject hellfirePoint;

    private void Awake()
    {
        bossStateManager = new BossStateManager();

        idleState = new IdleState(this, bossStateManager);
        dashState = new DashState(this, bossStateManager);
        stunState = new StunState(this, bossStateManager);
        chargeState = new ChargeState(this, bossStateManager);
        meleeAttackState = new MeleeAttackState(this, bossStateManager);
        specialAttackState = new SpecialAttackState(this, bossStateManager);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        bossStateManager.states = new BossAbstractState[] { chargeState, meleeAttackState, stunState, dashState, idleState, dashState, specialAttackState, dashState, idleState, dashState };
        bossStateManager.Initialize(bossStateManager.states[0]);

        specialAttacks = new System.Action[] { CastSpikeGrowth, CastPirouette, CastFirewall, CastHellfire };
        animationTriggers = new string[] { "spikeGrowth", "pirouette", "firewall", "hellfire" };
        castingTimes = new float[] { 2f, 1.5f, 2f, 1f };

        DeactivateSwordCollider();
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
            vent.GetComponent<VentDamage>().isActive = true;
        }
    }

    public void DeactivateBossCollider()
    {
        bossCollider.enabled = false;
    }

    public void ActivateBossCollider()
    {
        bossCollider.enabled = true;
    }

    public void DeactivateSwordCollider()
    {
        swordCollider.enabled = false;
    }

    public void ActivateSwordCollider()
    {
        swordCollider.enabled = true;
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
        StartCoroutine(CastTimer(castingTimes[randomIndex]));
    }

    IEnumerator CastTimer(float castTime)
    {
        yield return new WaitForSeconds(castTime);
        currentSpecialAttack();
        isCastingSpecialAttack = false;
    }

    private void CastSpikeGrowth()
    {
        Debug.Log("Boss special attack is: SPIKE GROWTH!");

        spikeTrap.transform.position = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
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
        }
    }

    private void ThrowDagger()
    {
        if (!canThrow) return;

        if (daggerIndex < daggers.Length - 1)
        {
            daggerIndex++;
        }
        else
        {
            daggerIndex = 0;
        }

        currentDagger = daggers[daggerIndex];

        currentDagger.SetActive(true);
        StartCoroutine(WaitBetweenThrows());
    }

    IEnumerator WaitBetweenThrows()
    {
        canThrow = false;
        yield return new WaitForSeconds(spinDuration / daggerAmount);
        canThrow = true;
    }

    private void CastFirewall()
    {
        Debug.Log("Boss' special attack is: FIREWALL!");

        firewall.transform.position = new Vector3(firewallPoint.transform.position.x, 0, firewallPoint.transform.position.z);

        Vector3 bossRotation = bossTransform.eulerAngles;
        firewall.transform.eulerAngles = new Vector3(
            firewall.transform.eulerAngles.x, bossRotation.y, firewall.transform.eulerAngles.z);
        firewall.SetActive(true);
        //StartCoroutine(WaitBeforeCreatingFireWall());
    }

    //IEnumerator WaitBeforeCreatingFireWall()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    fireWallBehaviour.isGrowing = true;
    //}

    private void CastHellfire()
    {
        Debug.Log("Boss' special attack is: HELLFIRE!");

        hellfire.transform.position = hellfirePoint.transform.position;
        hellfire.SetActive(true);
    }


}
