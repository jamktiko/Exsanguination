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
    [SerializeField] float timeBeforeSceneChange;
    [SerializeField] GameObject doneUI;

    private MusicManager musicManager;


    private void Awake()
    {
        background.color = new Color(0, 0, 0, 0);
    }

    private void Start()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }


    IEnumerator LevelTransition(float blackFadeTime)
    {
        doneUI.SetActive(false);
        // Start playing footsteps in the background
       StartCoroutine(musicManager.PlayFootstepsUntilTimerEnds());
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
        musicManager.isPlayingFootsteps = false;
        SceneManager.LoadScene(2);
    }

    public void LevelTransition()
    {
        StartCoroutine(LevelTransition(blackFadeTime));
    }
}
