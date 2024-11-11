using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    bool inArea;
    [SerializeField] public enum UtilityTool
    {
        Stake,
        GrapplingHook
    };

    [SerializeField] public UtilityTool tool;

    [SerializeField] PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
    }

    private void Start()
    {
        GetComponentInChildren<Light>().enabled = false;
        GetComponentInChildren<TMP_Text>().enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            inArea = true;

        GetComponentInChildren<Light>().enabled = true;
        GetComponentInChildren<TMP_Text>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponentInChildren<Light>().enabled = false;
        GetComponentInChildren<TMP_Text>().enabled = false;
    }

    public void StartPickUpItem()
    {
        if (inArea)
        {
            if (tool == UtilityTool.Stake)
                playerStats.foundStake = true;
            else if (tool == UtilityTool.GrapplingHook)
                playerStats.foundGrapplinghook = true;

            gameObject.SetActive(false);
        }
    }
}
