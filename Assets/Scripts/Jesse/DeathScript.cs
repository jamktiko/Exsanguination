using EmiliaScripts;
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

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(ExitToMainMenu);
        retryButton.onClick.AddListener(Retry);
    }

    void Start()
    {
        pauseScript = GameObject.Find("PauseManager").GetComponent<PauseScript>();
    }
    public void Die()
    {
        pauseScript.UnPauseGame();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathScreen.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        Debug.Log("pressed main menu");
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        Debug.Log("pressed retry");
        SceneManager.LoadScene(1);
    }

    private void OnEnable()
    {
        PlayerHealthManager playerHealthManager = GameObject.FindWithTag("HealthManager").GetComponent<PlayerHealthManager>();
        playerHealthManager.OnDeath += Die;
    }

}
