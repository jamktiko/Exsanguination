using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionToBossroom : MonoBehaviour
{
    PlayerStats playerStats;
    Image background;
    TextMeshProUGUI text;

    [SerializeField] float blackFadeTime;
    [SerializeField] float textFadeTime;

    [SerializeField] float timeBeforeTextAppears;
    [SerializeField] float timeBeforeSceneChange;
    void Awake()
    {
        playerStats = GameObject.FindWithTag("PlayerStats").GetComponent<PlayerStats>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        background = GetComponent<Image>();
        text.color = new Color(1,1,1,0);
        background.color = new Color(0, 0, 0, 0);
    }

    IEnumerator BossTransition(float blackFadeTime, TextMeshProUGUI text, float textFadeTime)
    {
        while (background.color.a <= 1)
        {
            background.color = new Color(0, 0, 0, background.color.a + Time.deltaTime / blackFadeTime);
            yield return null;
            Debug.Log(background.color.a);
        }


        yield return new WaitForSeconds(timeBeforeTextAppears);


        while (text.color.a <= 1)
        {
            text.color = new Color(1, 1, 1, text.color.a + Time.deltaTime / textFadeTime);
            yield return null;
        }

        yield return new WaitForSeconds(timeBeforeSceneChange);

        SceneManager.LoadScene(2);
    }

    public void TransitionToBoss()
    {
        StartCoroutine(BossTransition(2, text, 2));
    }
}
