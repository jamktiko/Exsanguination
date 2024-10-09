using EmiliaScripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    //Should only exist 1 per level
    private Vector3 respawnPos;

    private int playerHealth;
    private bool isLevelCleared;

    //NEED TO HAVE ITEMS/WEAPONS ETC SAVED

    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>().CurrentPlayerHealth();
        respawnPos = GameObject.FindGameObjectWithTag("RespawnPoint").transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Grab save stuff
            CheckPlayerHealthStatus();
            CheckLevelClearStatus();
        }
    }

    /// <summary>
    /// Should be called from restart button from menu or on death screen. Teleports the player, adjusts the health and reloads the level if it hasn't been cleared.
    /// </summary>
    public void StartLevelRestart()
    {

        //Set player health to save point health
        GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>().SetPlayerHealth(playerHealth);

        //if level isn't completed  -> Reload level
        if (!isLevelCleared)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            //Tp player to start of level, disable controller if using it
            //Need to ask Kimmo for function once player is gooddd

        }
    }

    /// <summary>
    /// Checks if the level is cleared of enemies and sets a bool to be true if completed
    /// </summary>
    private void CheckLevelClearStatus()
    {
        List<GameObject> list = new();
        list.AddRange(GameObject.FindGameObjectsWithTag("LevelStatus"));

        if (list.Count > 0)
        {
            foreach (GameObject go in list)
            {
                //isLevelCleared = go.GetComponent<LevelStatus>().isCleared;
            }
        }
    }

    private void CheckPlayerHealthStatus()
    {
        playerHealth = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>().CurrentPlayerHealth();
    }
}
