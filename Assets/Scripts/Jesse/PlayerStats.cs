using EmiliaScripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public bool foundSlaymore, foundGrapplinghook, foundStake, foundKeycard;
    [SerializeField] float preBossTime, bossTime, totalTime;
    [SerializeField] float timer;

    [SerializeField] TimeSpan preBoss, boss, total;

    PlayerHealthManager healthManager;

    [SerializeField] public string preBossTimeString;
    [SerializeField] public string bossTimeString;
    [SerializeField] public string totalTimeString;

    private void Awake()
    {
        healthManager = GameObject.FindWithTag("HealthManager").GetComponent<PlayerHealthManager>();

        healthManager.OnDeath += StopTimer;
        //boss death event += StopTimer;
        
        SceneManager.sceneLoaded += OnLevelLoad;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //Debug.Log(TimeInString(timer));
    }

    IEnumerator Timer()
    { //adds deltatime to timer variable until stopped
        while (true)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void RestartTimer()
    { //restarts timer from 0
        StopAllCoroutines();
        StartCoroutine(Timer());
        timer = 0;
    }

    public void StopTimer()
    {
        StopAllCoroutines();
        if (SceneManager.GetActiveScene().buildIndex == 2 )
        {
            bossTime = timer;
            totalTime = preBossTime + bossTime;
            bossTimeString = $"Boss completion time: {TimeInString(bossTime)}";
            totalTimeString = $"Total time: {TimeInString(totalTime)}";
        }
    }

    public void ResetSavedTimes()
    { //resets saved times
        preBossTime = bossTime = totalTime = 0;
    }

    private void OnLevelLoad(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.buildIndex == 2 && preBossTime == 0) 
        {
            preBossTime = timer;
            preBossTimeString = $"Level completion time: {TimeInString(preBossTime)}";
        }
        RestartTimer();
        if (scene.buildIndex == 1)
        { //set all found bools to false and fully resets saved times if in base level
            foundGrapplinghook = foundKeycard = foundSlaymore = foundStake = false;
            ResetSavedTimes();
        }
    }

    public string TimeInString(float time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}:{2:00}.{3:000}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
    }
}