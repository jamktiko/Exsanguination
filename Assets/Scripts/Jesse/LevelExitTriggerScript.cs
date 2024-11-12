using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitTriggerScript : MonoBehaviour
{
    private PlayerStats playerStats;
    private TransitionToBossroom transition;
    void Awake()
    {
        playerStats = GameObject.FindWithTag("PlayerStats").GetComponent<PlayerStats>();
        transition = GameObject.FindWithTag("TransitionToBossroom").GetComponent<TransitionToBossroom>();
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && playerStats.foundKeycard)
        {
            transition.TransitionToBoss();
        }
    }

    //public void CheckExit()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}
}
