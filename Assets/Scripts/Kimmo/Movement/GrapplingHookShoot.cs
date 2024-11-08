using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GrapplingHookShoot : MonoBehaviour
{
    [Header("References")]
    PlayerMovement playerMovement;
    AudioManager audioManager;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;
    Animator playerAnimator;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject arrowProjectile;
    [SerializeField] GameObject arroProjectileSpot;
    [SerializeField] Rigidbody arrowRB;
    EnemyFinisher enemyFinisher;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    float overShootYAxis = 0;
    [SerializeField] float arrowSpeed;
    bool shootArrow;
    public bool isGrappling;
    [SerializeField] Vector3 grapplePoint;
    [SerializeField] Vector3 arrowPosition;
    [SerializeField] Vector3 arrowStartPosition;

    [Header("Cooldown")]
    public float grapplingCd;
    float grapplingCdTimer;

    bool hasPlayedReadyAudioClip;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        //grappleCooldown = GameObject.Find("GrappleCooldown").GetComponent<GrappleCooldown>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();

        arrowStartPosition = arrowProjectile.transform.position;
        arrowRB = arrowProjectile.GetComponent<Rigidbody>();
        arrowProjectile.SetActive(false);
        enemyFinisher = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<EnemyFinisher>();
        hasPlayedReadyAudioClip = true;

        //arrowPosition = arrow.transform.position;
    }

    private void Update()
    {
        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }

        if (shootArrow)
        {
            arrowProjectile.transform.position = Vector3.MoveTowards(arrowProjectile.transform.position, grapplePoint,
                arrowSpeed * Time.deltaTime);
        }

        if (grapplingCdTimer <= 0 && !isGrappling)
        {
            ResetArrow();

            if (!hasPlayedReadyAudioClip)
            {
                audioManager.PlayGrapplingHookReadyAudioClip();
                hasPlayedReadyAudioClip = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (isGrappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }

    public void StartGrapple()
    {
        if (grapplingCdTimer > 0 || isGrappling || enemyFinisher.isFinishing) return;
        audioManager.PlayGrapplingHookShootAudioClip();
        playerAnimator.SetBool("grapple", true);

        grapplingCdTimer = grapplingCd;
        isGrappling = true;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            audioManager.PlayGrapplingHookHitAudioClip();
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        arrow.SetActive(false);
        arrowProjectile.SetActive(true);
        arrowProjectile.transform.SetParent(null);
        arrowRB.isKinematic = false;
        shootArrow = true;

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3 (transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overShootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overShootYAxis;

        playerMovement.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), grapplingCd);
    }

    public void StopGrapple()
    {
        playerAnimator.SetBool("grapple", false);

        shootArrow = false;
        isGrappling = false;
        lr.enabled = false;
        hasPlayedReadyAudioClip = false;

        playerMovement.ResetRestricitons();
    }

    private void ResetArrow()
    {
        arrow.SetActive(true);
        arrowProjectile.transform.SetParent(transform);
        arrowProjectile.transform.position = arrowStartPosition;
        arrowRB.isKinematic = true;
        arrowProjectile.SetActive(false);
    }
}
