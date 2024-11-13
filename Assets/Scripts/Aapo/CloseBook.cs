using TMPro;
using UnityEngine;

public class CloseBook : MonoBehaviour
{
    [SerializeField] InputHandler inputManager;
    [SerializeField] GameObject[] scrolls;
    [SerializeField] GameObject scrollData;



 

    public void CloseOtherBooks()
    {
        foreach (var scroll in scrolls)
        {
            scroll.SetActive(false);
        }
    }

    public void CloseBookButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputManager.EnableInput();
        Time.timeScale = 1f;
        CloseOtherBooks();
        scrollData.SetActive(false);
    }
}
