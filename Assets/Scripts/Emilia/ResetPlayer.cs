using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    [SerializeField] Transform[] resetSpot;
    GameObject player;
    public bool hasTriggered;
    public bool isTutorial;
    public bool isGrappleRoom;
    public bool isCharlesHyppyHuone;

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
            else
            {
                if (!hasTriggered)
                {
                    player.transform.SetPositionAndRotation(resetSpot[0].position, Quaternion.identity);
                    if (!isTutorial)
                    {
                        player.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                        player.transform.rotation = Quaternion.Euler(0, 45, 0);

                }

               

            }
           
        }
    }
}
