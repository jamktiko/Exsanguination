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

    [SerializeField] SettingsMenu settingsMenu;

    [SerializeField] public bool paused;
    [SerializeField] ControllerHandler controllerHandler;
    [SerializeField] InputHandler inputManager;
    void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
        continueButton.onClick.AddListener(UnPauseGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(ExitToMainMenu);
    }

    private void Start()
    {
        UnPauseGame();
    }

    public void DisableButtons()
    {
        inputManager.DisableInput();
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
        inputManager.DisableInput(); //how to press buttons if input is disabled on pause? Maybe instead a bool to stop moving things?
    }

    public void UnPauseGame()
    {
        Debug.Log("game unpaused");
        Time.timeScale = 1f;
        paused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputManager.EnableInput();
    }

    public void OpenSettings()
    {
        Debug.Log("settings opened");
        settingsMenu.OpenSettings(pauseMenu);
    }

    public void ExitToMainMenu()
    {
        Debug.Log("exited");
        inputManager.EnableInput();
        SceneManager.LoadScene(0);
    }
}
