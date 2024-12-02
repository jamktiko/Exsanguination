using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    [SerializeField] SettingsMenu settingsMenu;
    [SerializeField] GameObject mainMenu;
    private ControllerHandler controllerHandler;
    [SerializeField] EventSystem eventSystem;
    private bool controllerConnected;
    [SerializeField] MusicManager musicManager;
    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(ExitGame);

        controllerHandler = GetComponent<ControllerHandler>();
    }

    private void Start()
    {
        if (!controllerHandler.controllerIsConnected)
        {
            StartCoroutine(DelayFirstActionWithoutController());

        }

        else
        {
            StartCoroutine(DelayFirstActionWithController());


        }

    }
    private IEnumerator DelayFirstActionWithoutController()
    {
        yield return null; // Wait one frame
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator DelayFirstActionWithController()
    {
        yield return null; // Wait one frame
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
    
        if (controllerHandler.controllerIsConnected && controllerConnected == false)
        {
            eventSystem.SetSelectedGameObject(startButton.gameObject);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            controllerConnected = true;

        }

        if (!controllerHandler.controllerIsConnected && controllerConnected == true)
        {
            eventSystem.SetSelectedGameObject(null);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            controllerConnected = false;

        }
    }


    public void StartGame()
    {
        musicManager.PlayLevelMusic();
        SceneManager.LoadScene(1);
    }

    private void OpenSettings()
    {
        settingsMenu.OpenSettings(mainMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
