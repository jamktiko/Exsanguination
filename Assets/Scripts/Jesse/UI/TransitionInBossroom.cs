using System.Collections;
using TMPro;
using UnityEngine;
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

    void Awake()
    {
        background = GetComponent<Image>();

        topText = GetComponentsInChildren<TextMeshProUGUI>()[0];
        bottomText = GetComponentsInChildren<TextMeshProUGUI>()[1];

        topText.color = bottomText.color = new Color(1,1,1,0);

        StartCoroutine(TransitionInBoss());
    }

    IEnumerator TransitionInBoss()
    {
        yield return new WaitForSeconds(timeBeforeTopText);

        while (topText.color.a <= 1)
        {
            topText.color = new Color(1,1,1, topText.color.a + Time.deltaTime / topTextFadeInTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while (bottomText.color.a <= 1)
        {
            bottomText.color = new Color(1, 1, 1, bottomText.color.a + Time.deltaTime / bottomTextFadeInTime);
            yield return null;
        }

        yield return new WaitForSeconds(timeBeforeFadeOut);

        //allow moving again here?

        while (background.color.a >= 0)
        {
            bottomText.color = topText.color = new Color(1, 1, 1, topText.color.a - Time.deltaTime / fadeOutTime);
            background.color = new Color(0,0,0, background.color.a - Time.deltaTime / fadeOutTime);
            yield return null;
        }
    }
}
