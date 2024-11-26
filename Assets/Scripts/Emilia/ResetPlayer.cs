using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    [SerializeField] Transform[] resetSpot;
    GameObject player;
    public bool hasTriggered;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasTriggered)
            {
                player.transform.SetPositionAndRotation(resetSpot[0].position, Quaternion.identity);
            }

            else
            {
                player.transform.SetPositionAndRotation(resetSpot[1].position, Quaternion.identity);
            }
        }
    }
}
