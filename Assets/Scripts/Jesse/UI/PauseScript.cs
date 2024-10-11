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
    void Awake()
    {
        continueButton.onClick.AddListener(UnPauseGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(ExitToMainMenu);
    }

    private void Start()
    {
        UnPauseGame();
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
    }

    public void UnPauseGame()
    {
        Debug.Log("game unpaused");
        Time.timeScale = 1f;
        paused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        Debug.Log("settings opened");
        settingsMenu.OpenSettings(gameObject);
    }

    public void ExitToMainMenu()
    {
        Debug.Log("exited");
        SceneManager.LoadScene(0);
    }
}
