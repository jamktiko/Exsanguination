using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button continueButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] GameObject generalButton;

    [SerializeField] SettingsMenu settingsMenu;
    [SerializeField] GameObject victoryScreen;

    [SerializeField] public bool paused;
    private ControllerHandler controllerHandler;
    private InputHandler inputHandler;
    MusicManager musicManager;
    void Awake()
    {
        inputHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
        controllerHandler = GameObject.Find("InputManager").GetComponent<ControllerHandler>();
        continueButton.onClick.AddListener(UnPauseGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(ExitToMainMenu);


    }

    private void Start()
    {
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
        UnPauseGame();
        victoryScreen.SetActive(false);
    }

    public void DisableButtons()
    {
        inputHandler.DisableInput();
    }

    public void PauseGame()
    {
        Debug.Log("game paused");
        Time.timeScale = 0f;
        paused = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        if (!controllerHandler.controllerIsConnected) 
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
        inputHandler.DisableInput(); //how to press buttons if input is disabled on pause? Maybe instead a bool to stop moving things?
    }

    public void UnPauseGame()
    {
        Debug.Log("game unpaused");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputHandler.EnableInput();
        Time.timeScale = 1f;
        paused = false;
        settingsMenu.CloseSettings();
        pauseMenu.SetActive(false);
        
    }
    public void SetFirstButtonInPauseMenu()
    {
        StartCoroutine(DelaySetFirstButton());

    }
    private IEnumerator DelaySetFirstButton()
    {
        yield return null; // Wait one frame
        inputHandler.SetFirstButton(continueButton.gameObject);
    }

    
    public void OpenSettings()
    {
        Debug.Log("settings opened");
        settingsMenu.OpenSettings(pauseMenu);
    }

    public void ExitToMainMenu()
    {
        Debug.Log("exited");
        inputHandler.EnableInput();
        musicManager.PlayMenuDeathMusic();
        SceneManager.LoadScene(0);
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }
}
