using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    //[SerializeField] float moveSpeed;
    //[SerializeField] Transform[] waypoints;
    //public Transform playerTransform;
    //public Vector3 targetPosition;
    //float minDistance = 5f; // Minimum distance from player to exclude waypoint

    //private void Awake()
    //{
    //    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    //}

    //public void ChooseWaypoint()
    //{
    //    List<Transform> availableWaypoints = new List<Transform>();

    //    foreach (Transform waypoint in waypoints)
    //    {
    //        if (Vector3.Distance(waypoint.position, playerTransform.position) >= minDistance)
    //        {
    //            availableWaypoints.Add(waypoint);
    //        }
    //    }

    //    targetPosition = availableWaypoints[Random.Range(0, availableWaypoints.Count)].position;
    //}

    //public void RotateTowardsTarget()
    //{
    //    Vector3 targetDirection = targetPosition - transform.position;
    //    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 10f * Time.deltaTime, 0f);
    //    transform.rotation = Quaternion.LookRotation(newDirection);
    //}

    //public void MoveTowardsTarget()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, targetPosition,
    //        moveSpeed * Time.deltaTime);
    //}
}
