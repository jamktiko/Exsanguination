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
    public GameObject arrow;
    Animator playerAnimator;
    Transform grapplingHooktransform;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    float overShootYAxis = 0;
    [SerializeField] float arrowSpeed;
    bool shootArrow;
    Vector3 grapplePoint;
    Vector3 arrowPosition;
    Vector3 arrowStartPosition;

    [Header("Cooldown")]
    public float grapplingCd;
    float grapplingCdTimer;

    bool isGrappling;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        //grappleCooldown = GameObject.Find("GrappleCooldown").GetComponent<GrappleCooldown>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();

        arrowStartPosition = arrow.transform.position;
        arrowPosition = arrowStartPosition;
    }

    private void Update()
    {
        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }

        if (shootArrow)
        {
            arrow.transform.SetParent(null);
            arrowPosition = Vector3.MoveTowards(arrowPosition, grapplePoint, arrowSpeed * Time.deltaTime);

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
        if (grapplingCdTimer > 0 || isGrappling) return;
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

        shootArrow = true;
        lr.enabled = true;
        lr.SetPosition(1, arrowPosition);
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
        arrowPosition = arrowStartPosition;
        isGrappling = false;
        lr.enabled = false;

        playerMovement.ResetRestricitons();
        //playerAnimator.ResetTrigger("grapplefinished");
    }
}
