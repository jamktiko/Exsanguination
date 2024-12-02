using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionToLevel : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] float blackFadeTime;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clips;
    [SerializeField] float timeBeforeSceneChange;

    private bool isPlayingFootsteps = false;

    private void Awake()
    {
        background.color = new Color(0, 0, 0, 0);
    }

    IEnumerator PlayFootstepsUntilTimerEnds()
    {
        isPlayingFootsteps = true;

        while (isPlayingFootsteps)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.Play();

            // Wait for the clip to finish
            yield return new WaitForSecondsRealtime(clip.length);

            // Add a 0.4-second cooldown before playing the next clip
            yield return new WaitForSecondsRealtime(0.8f);
        }
    }

    IEnumerator LevelTransition(float blackFadeTime)
    {
        // Start playing footsteps in the background
        StartCoroutine(PlayFootstepsUntilTimerEnds());

        playerInput.DeactivateInput();
        Time.timeScale = 0f;
        background.enabled = true;
        // Fade the screen to black
        while (background.color.a <= 1)
        {
            background.color = new Color(0, 0, 0, background.color.a + Time.unscaledDeltaTime / blackFadeTime);
            yield return null;
        }

        // Wait for the scene change timer
        yield return new WaitForSecondsRealtime(timeBeforeSceneChange);

        // Stop the footsteps and transition to the next scene
        
        SceneManager.LoadScene(2);
    }

    public void LevelTransition()
    {
        StartCoroutine(LevelTransition(blackFadeTime));
    }
}
