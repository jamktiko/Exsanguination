using EmiliaScripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public bool foundSlaymore, foundGrapplinghook, foundStake, foundKeycard;
    [SerializeField] float tutorialTime, levelTime, bossTime, totalTime;
    [SerializeField] float timer;

    [SerializeField] TimeSpan tutorial, level, boss, total;

    PlayerHealthManager healthManager;
    public static PlayerStats playerStats;

    [SerializeField] public string tutorialTimeString;
    [SerializeField] public string levelTimeString;
    [SerializeField] public string bossTimeString;
    [SerializeField] public string totalTimeString;

    public bool cutSceneSeen;

    [SerializeField] TMP_Text totalTimeText;
    [SerializeField] TMP_Text totalTimeTextShadow;

    private Coroutine timerCoroutine;
    int deathCount;

    private void Awake()
    {
       
        
        SceneManager.sceneLoaded += OnLevelLoad;

        if (playerStats == null)
        {
            playerStats = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (playerStats != this)
        {
            Destroy(gameObject);
        }

        if (healthManager != null)
        {
            healthManager.OnDeath += RestartTimer;
        }
    }
    
    private void Update()
    {
        if (healthManager == null && SceneManager.GetActiveScene().buildIndex != 0)
        {
            healthManager = GameObject.FindWithTag("HealthManager").GetComponent<PlayerHealthManager>();

            healthManager.OnDeath += AddDeath;
            healthManager.OnDeath += RestartTimer;
        }
    }

    void AddDeath()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) { deathCount += 1; }
    }

    private void OnEnable()
    {
        if (healthManager != null)
        {
            healthManager.OnDeath += AddDeath;
            healthManager.OnDeath += StopTimer;

            //boss death event += StopTimer;
        }
    }

    private void OnDisable()
    {
        if (healthManager != null)
        {
            healthManager.OnDeath -= AddDeath;
            healthManager.OnDeath -= StopTimer;
        }
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

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timer = 0;
        timerCoroutine = StartCoroutine(Timer());
    }

    public void StopTimer()
    {
        Debug.Log("Time stopped");
        StopAllCoroutines();

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        if (SceneManager.GetActiveScene().buildIndex == 3 )
        {
            bossTime = timer;
            totalTime += bossTime;
            bossTimeString = $"Boss completion time: {TimeInString(bossTime)}";
            totalTimeString = $"Total time: {TimeInString(totalTime)}";
            totalTimeText.text = TimeInString(totalTime);
            totalTimeTextShadow.text = TimeInString(totalTime);
        }
    }

    public void ResetSavedTimes()
    { //resets saved times
        tutorialTime = levelTime = bossTime = totalTime = 0;
    }

    private void OnLevelLoad(Scene scene, LoadSceneMode sceneMode)
    {
        totalTimeText = GameObject.Find("GameCompletionTimeText").GetComponent<TextMeshProUGUI>();
        totalTimeTextShadow = totalTimeText.transform.Find("GameCompletionTimeTextShadow").GetComponent<TextMeshProUGUI>();

        if (scene.buildIndex == 3 && levelTime == 0)
        {
            levelTime = timer;
            totalTime += levelTime;
            levelTimeString = $"Level completion time: {TimeInString(levelTime)}";
        }
        if (scene.buildIndex == 2 && tutorialTime == 0)
        {
            tutorialTime = timer;
            totalTime += tutorialTime;
            tutorialTimeString = $"Level completion time: {TimeInString(tutorialTime)}";
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