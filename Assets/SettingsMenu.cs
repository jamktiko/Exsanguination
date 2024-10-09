using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] Button backButton;

    private GameObject activator;
    void Start()
    {
        backButton.onClick.AddListener(CloseSettings);
    }

    public void OpenSettings(GameObject activatorObject)
    {
        Debug.Log("opening settings");
        activator = activatorObject;
        gameObject.SetActive(true);
        activator.SetActive(false);
    }

    public void CloseSettings()
    {
        activator.SetActive(true);
        gameObject.SetActive(false);
    }
}