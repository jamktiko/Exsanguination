using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUILookAtPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.LookAt(2 * transform.position - player.transform.position);
    }
}
