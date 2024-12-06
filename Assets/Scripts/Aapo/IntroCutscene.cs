using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] MusicManager musicManager;

    TextMeshProUGUI topText;
     TextMeshProUGUI middleText;
     TextMeshProUGUI bottomText;


    [SerializeField] float timeBeforeTopText;

    [SerializeField] float topTextFadeInTime;
    [SerializeField] float middleTextFadeInTime;
    [SerializeField] float bottomTextFadeInTime;

    [SerializeField] float timeBeforeFadeOut;
    [SerializeField] float fadeOutTime;
    void Awake()
    {

        topText = GetComponentsInChildren<TextMeshProUGUI>()[0];
        middleText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        bottomText = GetComponentsInChildren<TextMeshProUGUI>()[2];
        topText.color = topText.color = new Color(1, 1, 1, 0);
        middleText.color = middleText.color = new Color(1, 1, 1, 0);
        bottomText.color = bottomText.color = new Color(1, 1, 1, 0);
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    private void Start()
    {
        background.enabled = false;
        topText.enabled = false;
        middleText.enabled = false;
        bottomText.enabled = false;
    }

    IEnumerator Cutscene()
        {
        background.enabled = true;
        topText.enabled = true;
        middleText.enabled = true;
        bottomText.enabled = true;
        Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(timeBeforeTopText);
            while (topText.color.a <= 1)
            {
                topText.color = new Color(1, 1, 1, topText.color.a + Time.unscaledDeltaTime / topTextFadeInTime);
                yield return null;
            }

            yield return new WaitForSecondsRealtime(1);

            while (middleText.color.a <= 1)
            {
                middleText.color = new Color(1, 1, 1, middleText.color.a + Time.unscaledDeltaTime / middleTextFadeInTime);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(1);
            while (bottomText.color.a <= 1)
            {
                bottomText.color = new Color(1, 1, 1, bottomText.color.a + Time.unscaledDeltaTime / bottomTextFadeInTime);
                yield return null;
            }
        yield return new WaitForSecondsRealtime(fadeOutTime);
            
            Time.timeScale = 1f;
        // Start playing the level music
        musicManager.PlayLevelMusic();

        // Load the scene while the intro music plays
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Optionally wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
    public void StartCutscene()
    {
        StartCoroutine(Cutscene());
    }
}

    

