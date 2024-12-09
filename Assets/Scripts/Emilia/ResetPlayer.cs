using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    [SerializeField] Transform[] resetSpot;
    GameObject player;
    public bool isTutorial;
    public bool isGrappleRoom;
    public bool isCharlesHyppyHuone;
    public bool standardRoom;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isGrappleRoom)
            {
                player.transform.SetPositionAndRotation(resetSpot[1].position, Quaternion.identity);
                player.transform.rotation = Quaternion.Euler(0, 180, 0);
                Debug.Log("grapple drop");
            }

            if (isCharlesHyppyHuone)
            {
                player.transform.SetPositionAndRotation(resetSpot[2].position, Quaternion.identity);
                player.transform.rotation = Quaternion.Euler(0, 180, 0);
                Debug.Log("grapple drop");
            }
            if (standardRoom)
            {
                player.transform.SetPositionAndRotation(resetSpot[0].position, Quaternion.identity);
                player.transform.rotation = Quaternion.Euler(0, 180, 0);
            }




        }

    }

}


