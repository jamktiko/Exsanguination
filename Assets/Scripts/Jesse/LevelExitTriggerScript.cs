using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelExitTriggerScript : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField] TransitionToBossroom transition;
    [SerializeField] Image transitionImage;
    private TMP_Text text;
    void Awake()
    {
        playerStats = GameObject.FindWithTag("PlayerStats").GetComponent<PlayerStats>();
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        text.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (playerStats.foundKeycard)
            {
                transitionImage.enabled = true;
                transition.TransitionToBoss();

            }
            else
            {
                text.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        text.enabled = false;
    }

    //public void CheckExit()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}
}
