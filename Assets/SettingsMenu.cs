using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] Button backButton;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] GameObject generalButton;
    [SerializeField] PauseScript pauseScript;
    private GameObject activator;
    [SerializeField] ControllerHandler controllerHandler;


    void Start()
    {
        backButton.onClick.AddListener(CloseSettings);
    }

    public void OpenSettings(GameObject activatorObject)
    {
        Debug.Log("opening settings");
        activator = activatorObject;
        gameObject.SetActive(true);
        activator.SetActive(false);
        if (controllerHandler.controllerIsConnected && SceneManager.GetActiveScene().buildIndex == 1) //CHANGE 1 to 2 when tutorial is being built!
        {
            StartCoroutine(DelaySetFirstButton());
        }
    }
    private IEnumerator DelaySetFirstButton()
    {
        yield return null; // Wait one frame
        inputHandler.SetFirstButton(generalButton);
    }

    public void CloseSettings()
    {
        if (controllerHandler.controllerIsConnected && SceneManager.GetActiveScene().buildIndex == 1) //CHANGE 1 to 2 when tutorial is being built!
        {
            pauseScript.SetFirstButtonInPauseMenu();
        }
        if (activator != null)
        {
            activator.SetActive(true);

        }
        gameObject.SetActive(false);
    }
}