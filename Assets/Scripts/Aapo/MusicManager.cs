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
    [SerializeField] AudioSource footstepAudioSource;
    [SerializeField] AudioClip[] footstepClips;
    public bool isPlayingFootsteps;

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
        SceneManager.sceneLoaded += OnLevelLoad;

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
        if (isMainMenu)
        {
            PlayMenuDeathMusic();
        }
    }

    private void Update()
    {
        if (!isMainMenu && deathScript.isDead && !hasDeathMusicStarted)
        {
            hasDeathMusicStarted = true;
            PlayMenuDeathMusic(fadeDuration: fadeDuration);
        }
    }

    public void PlayMenuDeathMusic(float fadeDuration = 1f)
    {
        Crossfade(menuDeathSource, fadeDuration);
    }

    public void PlayLevelMusic()
    {
        StartCoroutine(SequentialFade(menuDeathSource, levelIntroSource, fadeDuration, 1));
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
        if (levelManager.activeScene == 2 || levelManager.activeScene == 1)
        {
            Crossfade(levelIntroSource, fadeDuration);
        }
        if (levelManager.activeScene == 3)
        {
            PlayBossMusic();
        }
        if (levelManager.activeScene == 0)
        {
            PlayMenuDeathMusic();
        }
    }

    private IEnumerator BossIntroAndLoopCoroutine()
    {
        StartCoroutine(SequentialFade(levelLoopSource, bossIntroSource, fadeDuration, fadeDuration));

        // Wait for boss intro duration
        yield return new WaitForSecondsRealtime(bossIntroSource.clip.length);

        // Crossfade to first boss loop
        Crossfade(bossLoopSources[currentBossPhase], fadeDuration);
    }

    private IEnumerator SequentialFade(AudioSource fadeOutSource, AudioSource fadeInSource, float fadeOutDuration, float fadeInDuration)
    {
        if (fadeOutSource != null)
        {
            yield return FadeOut(fadeOutSource, fadeOutDuration);
        }

        if (fadeInSource != null)
        {
            FadeIn(fadeInSource, fadeInDuration);
        }
    }

    private void Crossfade(AudioSource newSource, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(CrossfadeCoroutine(newSource, duration));
    }

    private IEnumerator CrossfadeCoroutine(AudioSource newSource, float duration)
    {
        newSource.Play();
        newSource.volume = 0;

        float elapsedTime = 0f;
        AudioSource[] allSources = { menuDeathSource, levelIntroSource, levelLoopSource, bossIntroSource };
        allSources = System.Array.FindAll(allSources, s => s != null && s != newSource);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            newSource.volume = Mathf.Lerp(0, 0.5f, t);

            foreach (var source in allSources)
            {
                source.volume = Mathf.Lerp(0.5f, 0, t);
            }

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        newSource.volume = 0.5f;
        foreach (var source in allSources)
        {
            source.Stop();
            source.volume = 0;
        }
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            source.volume = Mathf.Lerp(startVolume, 0, t);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        source.Stop();
        source.volume = 0;
    }

    private void FadeIn(AudioSource source, float duration)
    {
        StartCoroutine(FadeInCoroutine(source, duration));
    }

    private IEnumerator FadeInCoroutine(AudioSource source, float duration)
    {
        source.Play();
        source.volume = 0;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            source.volume = Mathf.Lerp(0, 0.5f, t);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        source.volume = 0.5f;
    }

    private void OnLevelLoad(Scene scene, LoadSceneMode sceneMode)
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

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

}