using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionToBossroom : MonoBehaviour
{
   [SerializeField] PlayerStats playerStats;
    [SerializeField] Image background;
    [SerializeField] TMP_Text text;
    [SerializeField] PlayerInput playerInput;


[SerializeField] float blackFadeTime;
    [SerializeField] float textFadeTime;

    [SerializeField] float timeBeforeTextAppears;
    [SerializeField] float timeBeforeSceneChange;

    private void Awake()
    {
        text.color = new Color(1, 1, 1, 0);
        background.color = new Color(0, 0, 0, 0);
    }


    IEnumerator BossTransition(float blackFadeTime, TMP_Text text, float textFadeTime)
    {
       
        playerInput.DeactivateInput();    
        Time.timeScale = 0f;
        while (background.color.a <= 1)
        {
            background.color = new Color(0, 0, 0, background.color.a + Time.unscaledDeltaTime / blackFadeTime);
            yield return null;
            Debug.Log(background.color.a);
        }


        yield return new WaitForSecondsRealtime(timeBeforeTextAppears);


        while (text.color.a <= 1)
        {
            text.color = new Color(1, 1, 1, text.color.a + Time.unscaledDeltaTime / textFadeTime);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(timeBeforeSceneChange);

        SceneManager.LoadScene(2);
    }

    public void TransitionToBoss()
    {
        StartCoroutine(BossTransition(2, text, 2));
    }
}
