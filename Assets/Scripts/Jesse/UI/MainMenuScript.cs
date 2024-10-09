using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    [SerializeField] SettingsMenu settingsMenu;
    [SerializeField] GameObject mainMenu;

    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(ExitGame);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    

    public void StartGame()
    {
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
