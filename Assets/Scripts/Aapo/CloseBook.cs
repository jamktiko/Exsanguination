using TMPro;
using UnityEngine;

public class CloseBook : MonoBehaviour
{
    InputHandler inputManager;
    [SerializeField] GameObject[] scrolls;
    [SerializeField] GameObject scrollData;

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
    }

    private void Start()
    {
        foreach (var scroll in scrolls)
        {
            scroll.SetActive(false);
        }
        scrollData.SetActive(false);
    }


    public void CloseBookButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputManager.EnableInput();
        Time.timeScale = 1f;
        foreach (var scroll in scrolls)
        {
            scroll.SetActive(false);
        }
        scrollData.SetActive(false);
    }
}
