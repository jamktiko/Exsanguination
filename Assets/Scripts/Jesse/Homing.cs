using UnityEngine;

public class Homing : MonoBehaviour
{
            [Header("Tracking boxcast")]
    [Tooltip("Size of tracking boxCast")]
    [SerializeField] private Vector3 boxCastSize;

    [Tooltip("LayerMask for trackable objects")]
    [SerializeField] private LayerMask trackingLayer;

            [Header("Tracking")]
    [Tooltip("Used for Lerp with turning towards target. 0 - 1. 0.1 is pretty smooth!")]
    [SerializeField] private float turnSpeed;

    [Tooltip("Allows tracking (rotation and movement)")]
    [SerializeField] public bool tracking = true;

    private Vector3 targetPreviousPosition;

    [Tooltip("Tracks first detected target forever, otherwise tracks whatever is detected in BoxCast")]
    [SerializeField] bool permamentLock = true;

    private Transform targetObject;

            [Header("Movement")]
    [Tooltip("Speed at which object moves forward.")]
    [SerializeField] float speed;

    void Update()
    {
        if (targetObject == null)
        {
            targetObject = rayHit();
            Move();
            return;
        }
        if (permamentLock)
        {
            RotateTowardsTarget(targetObject);
        }
        else if (rayHit() != null)
        {
            RotateTowardsTarget(rayHit());
        }
        Move();
    }

    void RotateTowardsTarget(Transform target)
    { //rotates object towards target
        Vector3 targetPredict = (target.position + TargetVelocity(target));
        Vector3 targetDirection;
        //if (Vector3.Distance(transform.position, target.position) < speed / 4)
        //{
        //    targetPredict = target.position;
        //}
        targetDirection = targetPredict - transform.position;

        Debug.DrawLine(transform.position, targetPredict, Color.red);

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    void Move()
    { //moves object forwards
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    Transform rayHit()
    { //returns transform of hit object
        if (Physics.BoxCast(transform.position - boxCastSize.z * transform.forward, new Vector3(boxCastSize.x, boxCastSize.y, 0) * 0.5f, transform.forward, out RaycastHit hit, transform.rotation, boxCastSize.z, trackingLayer))
        {
            return hit.transform;
        }
        else { return null; }
    }

    Vector3 TargetVelocity(Transform target)
    { //calculates velocity of target without using rigidbody to use in "leading"
        Vector3 targetVelocity = (target.position - targetPreviousPosition) / Time.deltaTime;
        targetPreviousPosition = target.position;
        return targetVelocity;
    }

    private void OnDrawGizmos()
    {
        //this thing doesnt rotate with the boxcast because that code would be 14 elephants in length. Also actual boxcast is around 1 unit further than this because idfk
        Gizmos.color = Color.magenta;

        Vector3 cubePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Gizmos.DrawWireCube(cubePosition, new Vector3(boxCastSize.x, boxCastSize.y, boxCastSize.z));
    }
}