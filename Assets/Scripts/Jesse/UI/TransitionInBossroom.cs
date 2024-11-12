using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TransitionInBossroom : MonoBehaviour
{
    Image background;

    TextMeshProUGUI topText;
    TextMeshProUGUI bottomText;

    [SerializeField] float timeBeforeTopText;

    [SerializeField] float topTextFadeInTime;
    [SerializeField] float bottomTextFadeInTime;

    [SerializeField] float timeBeforeFadeOut;
    [SerializeField] float fadeOutTime;
    private PlayerInput playerInput;


    void Awake()
    {
        background = GetComponent<Image>();

        topText = GetComponentsInChildren<TextMeshProUGUI>()[0];
        bottomText = GetComponentsInChildren<TextMeshProUGUI>()[1];

        topText.color = bottomText.color = new Color(1,1,1,0);
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

    }

    private void Start()
    {
        StartCoroutine(TransitionInBoss());
    }

    IEnumerator TransitionInBoss()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        Time.timeScale = 0f;
        playerInput.DeactivateInput();
        yield return new WaitForSecondsRealtime(timeBeforeTopText);
        while (topText.color.a <= 1)
        {
            topText.color = new Color(1,1,1, topText.color.a + Time.unscaledDeltaTime / topTextFadeInTime);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1);

        while (bottomText.color.a <= 1)
        {
            bottomText.color = new Color(1, 1, 1, bottomText.color.a + Time.unscaledDeltaTime / bottomTextFadeInTime);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(timeBeforeFadeOut);

        
        while (background.color.a >= 0)
        {
            bottomText.color = topText.color = new Color(1, 1, 1, topText.color.a - Time.unscaledDeltaTime / fadeOutTime);
            background.color = new Color(0,0,0, background.color.a - Time.unscaledDeltaTime / fadeOutTime);
            yield return null;
        }

        playerInput.ActivateInput();
        Time.timeScale = 1f;

    }
}
