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

        // Ensure the loop source is set to loop
        loopSource.loop = true;

        // Start coroutine to handle timing
        StartCoroutine(PlayLoopAfterIntro());
    }

    private System.Collections.IEnumerator PlayLoopAfterIntro()
    {
        // Calculate when to start the loop
        float introEndTime = Time.unscaledTime + introSource.clip.length + introToLoopDelay;

        // Wait until the unscaled time reaches the calculated end time
        while (Time.unscaledTime < introEndTime)
        {
            yield return null;
        }

        // Play the looping audio
        loopSource.Play();
    }

    private void Update()
    {
        if (deathScript.isDead && !hasDeathMusicStarted)
        {
            hasDeathMusicStarted = true;
            Debug.Log("dead");

            // Stop other music and play death music
            deathSource.Play();
            loopSource.Pause();
            introSource.Pause();
        }
    }
}
