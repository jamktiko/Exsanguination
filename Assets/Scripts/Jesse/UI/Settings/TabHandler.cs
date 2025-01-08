using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour
{
    [SerializeField] Button[] tabButtons = new Button[4];
    [SerializeField] InputHandler inputHandler;
    [SerializeField] GameObject[] tabs = new GameObject[4];
    private GameObject activeTab;
    [SerializeField] GameObject[] firstOptions = new GameObject[4];
    [SerializeField] Image[] arrowImages = new Image[3]; // Assuming you have an array of Image references for arrows
    public EventSystem eventSystem;
    [SerializeField] Image mouseArrow;
    [SerializeField] ControllerHandler controllerHandler;

    

    void Start()
    {
        // Set up button listeners
        tabButtons[0].onClick.AddListener(() => ChangeTab(0));
        tabButtons[1].onClick.AddListener(() => ChangeTab(1));
        tabButtons[2].onClick.AddListener(() => ChangeTab(2));
        tabButtons[3].onClick.AddListener(() => ChangeTab(3));

        activeTab = tabs[0];
        activeTab.SetActive(true);
    }

    


    public void ChangeTab(int tabNumber)
    {
        activeTab.SetActive(false);
        tabs[tabNumber].SetActive(true);
        activeTab = tabs[tabNumber];
        if (controllerHandler.controllerIsConnected && SceneManager.GetActiveScene().buildIndex == 2) 
        {
            StartCoroutine(DelaySetFirstButton(tabNumber));
        }
    }

    private IEnumerator DelaySetFirstButton(int tabNumber)
    {
        yield return null; // Wait one frame
        inputHandler.SetFirstButton(firstOptions[tabNumber]);

    }
}
