using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource menuDeathSource; // For main menu and death music
    [SerializeField] private AudioSource levelIntroSource; // For level intro music
    [SerializeField] private AudioSource levelLoopSource; // For level looping music
    [SerializeField] private AudioSource bossIntroSource; // For boss intro music
    [SerializeField] private AudioSource[] bossLoopSources; // Variations for boss loops

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1f; // Default fade time in seconds
    [SerializeField] private DeathScript deathScript; // Reference to death detection
    [SerializeField] private float introToLoopDelay = 0f; // Delay between intro and loop music

    private static MusicManager instance;
    private bool hasDeathMusicStarted;
    private bool isBossMusicActive;
    private int currentBossPhase;

    public bool isMainMenu;
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        // Singleton pattern for DontDestroyOnLoad
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (isMainMenu)
        {
            PlayMenuDeathMusic();
        }
    }

    private void Update()
    {
        if (!isMainMenu)
        {
            if (deathScript.isDead && !hasDeathMusicStarted)
            {
                hasDeathMusicStarted = true;
                PlayMenuDeathMusic(fadeDuration: 0.5f); // Crossfade to menu/death music
            }
        }
    }

    public void PlayMenuDeathMusic(float fadeDuration = 1f)
    {
        Crossfade(menuDeathSource, fadeDuration);
    }

    public void PlayLevelMusic()
    {
        Crossfade(levelIntroSource, fadeDuration);
    }

    

    public void PlayBossMusic()
    {
        if (isBossMusicActive) return;

        isBossMusicActive = true;
        currentBossPhase = 0; // Start from the first variation
        StartCoroutine(BossIntroAndLoopCoroutine());
    }

    public void ChangeBossMusicVariation(int phase)
    {
        if (phase < 0 || phase >= bossLoopSources.Length || phase == currentBossPhase) return;

        currentBossPhase = phase;
        Crossfade(bossLoopSources[phase], fadeDuration);
    }

    public void OnPlayerRetry()
    {
        hasDeathMusicStarted = false;
        if (levelManager.activeScene == 2)
        {
            PlayLevelMusic(); // Restart level music
        }
        else
        {
            PlayBossMusic(); // Restart boss music
        }
    }

    private IEnumerator LevelMusicSequenceCoroutine()
    {
        // Crossfade to level intro
        Crossfade(levelIntroSource, fadeDuration);

        // Wait for intro to finish
        yield return new WaitForSecondsRealtime(levelIntroSource.clip.length + introToLoopDelay);

        // Crossfade to level loop
        Crossfade(levelLoopSource, fadeDuration);
    }

    private IEnumerator BossIntroAndLoopCoroutine()
    {
        // Crossfade to boss intro
        Crossfade(bossIntroSource, fadeDuration);

        // Wait for intro duration
        yield return new WaitForSecondsRealtime(bossIntroSource.clip.length);

        // Crossfade to first boss loop
        Crossfade(bossLoopSources[currentBossPhase], fadeDuration);
    }

    private void Crossfade(AudioSource newSource, float duration)
    {
        StopAllCoroutines(); // Stop ongoing crossfade coroutines
        StartCoroutine(CrossfadeCoroutine(newSource, duration));
    }

    private IEnumerator CrossfadeCoroutine(AudioSource newSource, float duration)
    {
        // Start new source
        newSource.Play();
        newSource.volume = 0;

        // Gradually fade in the new source and fade out others
        float elapsedTime = 0f;
        AudioSource[] allSources = { menuDeathSource, levelIntroSource, levelLoopSource, bossIntroSource };
        allSources = System.Array.FindAll(allSources, s => s != null && s != newSource);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            newSource.volume = Mathf.Lerp(0, 1, t);

            foreach (var source in allSources)
            {
                source.volume = Mathf.Lerp(1, 0, t);
            }

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure volumes are set
        newSource.volume = 1;
        foreach (var source in allSources)
        {
            source.Stop(); // Stop other sources
            source.volume = 0;
        }
    }
}
