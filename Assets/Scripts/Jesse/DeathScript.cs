using EmiliaScripts;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;
    PauseScript pauseScript;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button retryButton;
    public bool isDead;
    private ControllerHandler controllerHandler;
    private InputHandler inputHandler;
    [SerializeField] MusicManager musicManager;
    private void Awake()
    {
        mainMenuButton.onClick.AddListener(ExitToMainMenu);
        retryButton.onClick.AddListener(Retry);
        pauseScript = GameObject.Find("PauseManager").GetComponent<PauseScript>();
        controllerHandler = GameObject.Find("InputManager").GetComponent<ControllerHandler>();
        inputHandler = GameObject.Find("Player").GetComponent<InputHandler>();
    }

    private void Start()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        deathScreen.SetActive(false);
    }

    public void Die()
    {
        isDead = true;
        pauseScript.UnPauseGame();
        Time.timeScale = 0f;
        pauseScript.DisableButtons();
        if (!controllerHandler.controllerIsConnected)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        deathScreen.SetActive(true);
        StartCoroutine(DelaySetFirstButton());
    }
    private IEnumerator DelaySetFirstButton()
    {
        yield return null; // Wait one frame
        inputHandler.SetFirstButton(retryButton.gameObject);
    }

    public void ExitToMainMenu()
    {
        isDead=false;
        Debug.Log("pressed main menu");
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        isDead = false;
        Debug.Log("pressed retry");
        musicManager.OnPlayerRetry();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        PlayerHealthManager playerHealthManager = GameObject.FindWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        playerHealthManager.OnDeath += Die;
    }

}
