using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource loopSource;
    [SerializeField] private AudioSource deathSource;
    [SerializeField] DeathScript deathScript;
    // Editable delay between intro and loop in seconds
    [SerializeField] private float introToLoopDelay = 0f;

    private bool isPaused;
    public bool isBossScene;
    private bool hasDeathMusicStarted;

    private void Start()
    {
        // Start playing the intro clip
        introSource.Play();

        // Set loopSource to not play yet
        loopSource.loop = true;

        // Wait for intro to finish, plus the editable delay, and then start the loop
        Invoke("StartLoop", introSource.clip.length + introToLoopDelay);
    }

    private void StartLoop()
    {
        loopSource.Play();
    }

    private void Update()
    {
        if (deathScript.isDead && !hasDeathMusicStarted)
        {
            hasDeathMusicStarted = true;
            Debug.Log("dead");
            deathSource.Play();
            loopSource.Pause();
            introSource.Pause();

        }
    }

    //{
    //    // Check if the game is paused (Time.timeScale == 0)
    //    if (Time.timeScale == 0 && !isPaused && !isBossScene)
    //    {
    //        // Pause the music when the game is paused
    //        PauseMusic();
    //        isPaused = true;
    //    }
    //    else if (Time.timeScale != 0 && isPaused)
    //    {
    //        // Resume the music when the game is unpaused
    //        ResumeMusic();
    //        isPaused = false;
    //    }
    //}

    //private void PauseMusic()
    //{
    //    // Pause both intro and loop music
    //    if (introSource.isPlaying)
    //    {
    //        introSource.Pause();
    //    }
    //    if (loopSource.isPlaying)
    //    {
    //        loopSource.Pause();
    //    }

    //    if (deathScript.isDead)
    //    {
    //        deathSource.Play();
    //        loopSource.Pause();
    //        introSource.Pause();

    //    }
    //}

    //private void ResumeMusic()
    //{
    //    // Resume both intro and loop music if they were paused
    //    if (!introSource.isPlaying)
    //    {
    //        introSource.UnPause();
    //    }
    //    if (!loopSource.isPlaying)
    //    {
    //        loopSource.UnPause();
    //    }
    //}
}
