using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button continueButton;
    [SerializeField] Button mainMenuButton;

    private bool paused;
    void Awake()
    {
        mainMenuButton.onClick.AddListener(ExitToMainMenu);
        continueButton.onClick.AddListener(UnPauseGame);
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

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
