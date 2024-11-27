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
    private TMP_Text[] texts;
    public bool isTutorialRoom;
    void Awake()
    {
        playerStats = GameObject.FindWithTag("PlayerStats").GetComponent<PlayerStats>();
        if (!isTutorialRoom)
        {
            texts = GetComponentsInChildren<TMP_Text>();
        }
    }

    private void Start()
    {
        if (!isTutorialRoom)
        {
            foreach (var text in texts)
            {
                text.enabled = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!isTutorialRoom)
            {
                if (playerStats.foundKeycard)
                {
                    transitionImage.enabled = true;
                    transition.TransitionToBoss();

                }
                else
                {
                    foreach (var text in texts)
                    {
                        text.enabled = true;
                    }
                }
            }
            else
            {
                SceneManager.LoadScene(2);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isTutorialRoom)
        {
            foreach (var text in texts)
            {
                text.enabled = false;
            }
        }
    }

    //public void CheckExit()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}
}
