using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager musicManager;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource menuDeathSource; // For main menu and death music
    [SerializeField] private AudioSource levelIntroSource; // For level intro music
    [SerializeField] private AudioSource levelLoopSource; // For level looping music
    [SerializeField] private AudioSource bossIntroSource; // For boss intro music
    [SerializeField] private AudioSource[] bossLoopSources; // Variations for boss loops
    [SerializeField] private AudioSource bossTransitionSource;
    public AudioSource currentSource;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1f; // Default fade time in seconds
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private DeathScript deathScript; // Reference to death detection
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip[] footstepClips;
    public bool isPlayingFootsteps;

    private bool isBossMusicActive;
    private int currentBossPhase;

    private void Awake()
    {
        if (musicManager == null)
        {
            musicManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (musicManager != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure starting on the correct music based on the current scene
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayMenuDeathMusic();
        }
    }

    // Play Menu/Death music, crossfading from any current track
    public void PlayMenuDeathMusic(float fadeDuration = 1f)
    {
        StopAllCoroutines();
        levelIntroSource.Stop();
        levelLoopSource.Stop();
        CrossfadeToSource(menuDeathSource, fadeDuration);
        
    }

    // Play Level music: Crossfade from current track to level intro, then immediately to loop
    public void PlayLevelMusic()
    {
        levelIntroSource.Stop();
        levelLoopSource.Stop();
        levelLoopSource.volume = 0.5f;
        CrossfadeToSource(levelIntroSource, fadeDuration);
        StartCoroutine(HandleLevelIntroAndLoop());
    }

    private IEnumerator HandleLevelIntroAndLoop()
    {
        if (!levelIntroSource.isPlaying)
        {
            levelIntroSource.Play();
        }
        // Wait for intro to finish before starting the loop music
        yield return new WaitForSecondsRealtime(levelIntroSource.clip.length);
        // Immediately switch to level loop without crossfade
        levelLoopSource.Play();
        levelIntroSource.Stop();
        currentSource = levelLoopSource;
    }

    // Play boss music: Crossfade to intro, then loop music with variations
    public void PlayBossMusic()
    {
        if (isBossMusicActive) return;

        isBossMusicActive = true;
        currentBossPhase = 0; // Start from the first variation
        StartCoroutine(HandleBossIntroAndLoop());
    }

    private IEnumerator HandleBossIntroAndLoop()
    {
        // Crossfade into the boss intro music first
        CrossfadeToSource(bossIntroSource, 3f);
        bossLoopSources[1].Play();
        // Wait for the intro to finish
        yield return new WaitForSecondsRealtime(bossIntroSource.clip.length-0.005f);
        bossLoopSources[0].Play();
        // Stop the intro music and start the first loop variation

        currentSource = bossLoopSources[0];
        
    }

    public void BossSecondPhase()
    {
        CrossfadeToSource(bossLoopSources[1], fadeDuration);
    }

    public void BossThirdPhase()
    { 
        StartCoroutine(HandleBossThirdPhase());
    }


    private IEnumerator HandleBossThirdPhase()
    {
        // Crossfade into the boss intro music first
        CrossfadeToSource(bossTransitionSource, fadeDuration);

        // Wait for the intro to finish
        yield return new WaitForSecondsRealtime(bossTransitionSource.clip.length);

        // Stop the intro music and start the first loop variation
        CrossfadeToSource(bossLoopSources[2], fadeDuration);

        currentSource = bossLoopSources[2];

    }


    // Crossfade from current track to a new one
    private void CrossfadeToSource(AudioSource newSource, float duration)
    {
        if (currentSource != null)
        {
            // If there is a current source, start fading it out while fading in the new source
            StartCoroutine(FadeOutAndIn(currentSource, newSource, duration));
        }
        else
        {
            // If there is no current source (initial track), just play the new track
            newSource.Play();
            newSource.volume = 0.5f; // Set a default volume for the new track
            currentSource = newSource; // Update the current source reference
        }
    }

    private IEnumerator FadeOutAndIn(AudioSource fadeOutSource, AudioSource fadeInSource, float duration)
    {
        // Save the starting volume of the old track (fade-out)
        float startVolume = fadeOutSource.volume;
        float elapsedTime = 0f;

        // Ensure the new source starts playing, but with zero volume initially
        fadeInSource.volume = 0f;
        fadeInSource.Play();

        // Fade out the old track and fade in the new track simultaneously
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            fadeOutSource.volume = Mathf.Lerp(startVolume, 0f, t);  // Fade out the current track
            fadeInSource.volume = Mathf.Lerp(0f, 0.5f, t);  // Fade in the new track
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // After the fade duration, stop the old track and ensure volumes are correct
        fadeOutSource.Stop();
        fadeOutSource.volume = 0f;

        // Set the final volume for the new track
        fadeInSource.volume = 0.5f;  // Set to your desired volume (can be adjusted)
        currentSource = fadeInSource;  // Update the current source to the new track
    }



    public void OnPlayerRetry()
    {
        if (levelManager.activeScene == 2 || levelManager.activeScene == 1)
        {
            PlayLevelMusic();
        }
        else if (levelManager.activeScene == 3)
        {
            PlayBossMusic();
        }
        else if (levelManager.activeScene == 0)
        {
            PlayMenuDeathMusic();
        }
    }

    // Footsteps method to play until the timer ends
    public IEnumerator PlayFootstepsUntilTimerEnds()
    {
        isPlayingFootsteps = true;

        while (isPlayingFootsteps)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepAudioSource.clip = clip;
            footstepAudioSource.Play();

            // Wait for the clip to finish
            yield return new WaitForSecondsRealtime(clip.length);

            // Add a 0.4-second cooldown before playing the next clip
            yield return new WaitForSecondsRealtime(0.8f);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        levelManager = GameObject.Find("LevelManager")?.GetComponent<LevelManager>();
        if (scene.buildIndex != 0)
        {
            deathScript = GameObject.FindGameObjectWithTag("DeathManager").GetComponent<DeathScript>();
        }
    }
}
