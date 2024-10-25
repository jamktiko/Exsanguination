using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GrapplingHookShoot : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement playerMovement;
    AudioManager audioManager;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    float overShootYAxis = 0;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    private bool isGrappling;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        //grappleCooldown = GameObject.Find("GrappleCooldown").GetComponent<GrappleCooldown>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
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

        grapplingCdTimer = grapplingCd;
        isGrappling = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
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
        isGrappling = false;

        lr.enabled = false;

        playerMovement.ResetRestricitons();
    }
}
