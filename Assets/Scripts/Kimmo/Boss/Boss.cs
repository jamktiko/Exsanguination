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

    public bool isStunned;
    public float stunDuration;
    public float idleDuration;

    System.Action[] specialAttacks;
    System.Action currentSpecialAttack;
    public bool isCastingSpecialAttack;
    [SerializeField] float castTime;

    [SerializeField] GameObject spikeTrap;
    [SerializeField] Vector3 spikeTrapStartingPosition;
    float spikeTrapDuration = 10f;

    [SerializeField] GameObject fireWall;

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

        //specialAttacks = new System.Action[] {CastSpikeGrowth, CastPirouette, CastFirewall, CastHellfire };

        specialAttacks = new System.Action[] { CastFirewall };
        DeactivateSwordCollider();
    }

    private void Update()
    {
        bossStateManager.currentBossState.FrameUpdate();
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
        var actualTargetPosition = new Vector3(targetPosition.x, 0, targetPosition.z);
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
        isCastingSpecialAttack = true;
        currentSpecialAttack = specialAttacks[Random.Range(0, specialAttacks.Length)];
        StartCoroutine(CastTimer());
    }

    IEnumerator CastTimer()
    {
        yield return new WaitForSeconds(castTime);
        currentSpecialAttack();
        isCastingSpecialAttack = false;
    }

    private void CastSpikeGrowth()
    {
        Debug.Log("Boss' special attack is: SPIKE GROWTH!");

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
        Debug.Log("Boss' special attack is: PIROUETTE!");


    }

    private void CastFirewall()
    {
        Debug.Log("Boss' special attack is: FIREWALL!");

        fireWall.transform.eulerAngles = new Vector3(
            fireWall.transform.eulerAngles.x, bossTransform.rotation.y, fireWall.transform.eulerAngles.z);
        fireWall.transform.position = new Vector3(bossTransform.position.x, 1, bossTransform.position.z);
    }

    private void CastHellfire()
    {
        Debug.Log("Boss' special attack is: HELLFIRE!");


    }
}
