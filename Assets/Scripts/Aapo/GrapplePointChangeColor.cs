using UnityEngine;

public class GrapplingPointChangeColor : MonoBehaviour
{
    private Ray ray;
    [SerializeField] float noticeDistance;
    private HookPointData lastHitHookPoint; // Tracks the last aimed-at hook point
    public bool activateRay;
    public LayerMask whatIsGrappleable;
    [SerializeField] Transform cam;

    private void Start()
    {
        this.enabled = false;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, noticeDistance, whatIsGrappleable))
        {
            var currentHookPoint = hit.collider.GetComponent<HookPointData>();

            // If the ray hits a new hook point, switch states
            if (currentHookPoint != lastHitHookPoint)
            {
                UpdateHookPointState(lastHitHookPoint, true);  // Reset the previous hook point
                UpdateHookPointState(currentHookPoint, false); // Highlight the new hook point
                lastHitHookPoint = currentHookPoint;
                Debug.Log("hit hookpoint");
            }
        }
        else
        {
            // If no hook point is hit, reset the last aimed hook point
            UpdateHookPointState(lastHitHookPoint, true);
            lastHitHookPoint = null;
        }
    }

    private void UpdateHookPointState(HookPointData hookPoint, bool isUnAimed)
    {
        if (hookPoint == null) return;

        hookPoint.unAimedObject?.SetActive(isUnAimed);
        hookPoint.aimedObject?.SetActive(!isUnAimed);
    }


}
