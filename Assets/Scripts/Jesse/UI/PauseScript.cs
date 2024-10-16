using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button continueButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button mainMenuButton;

    [SerializeField] SettingsMenu settingsMenu;

    [SerializeField] private bool paused;
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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && paused == false)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused == true)
        {
            UnPauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        paused = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        paused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        settingsMenu.OpenSettings(gameObject);
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
