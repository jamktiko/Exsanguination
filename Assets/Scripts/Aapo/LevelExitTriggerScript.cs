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
    [SerializeField] TransitionToLevel transitionToLevel;
    [SerializeField] Image transitionImage;
    [SerializeField] GameObject tutorialEnemy;
    private TMP_Text[] texts;
    public bool isTutorialRoom;
    [SerializeField] MusicManager musicManager;
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

       
            musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
        
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
                    musicManager.PlayBossMusic();
                    musicManager.PlayFootstepsUntilTimerEnds();
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
                if (!tutorialEnemy.activeSelf)
                {
                    transitionToLevel.LevelTransition();
                }
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
