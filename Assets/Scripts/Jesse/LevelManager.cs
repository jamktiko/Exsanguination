using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int activeScene;
    

    void Start()
    {
        activeScene = SceneManager.GetActiveScene().buildIndex;
        
    }


    void Update()
    {

    }

    public void CheckExit()
    {
        SceneManager.LoadScene(activeScene + 1);
    }
}
