using UnityEngine;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour
{
    [SerializeField] Button[] tabButtons = new Button[3];

    [SerializeField] GameObject[] tabs = new GameObject[3];

    private GameObject activeTab;

    void Start()
    {
        //for (int i = 0; i < tabs.Length; i++)
        //{
        //    Debug.Log($"i = {i} button = {tabButtons[i].gameObject.name}");
        //    tabButtons[i].onClick.AddListener(() => ChangeTab(i));
        //}
        
        tabButtons[0].onClick.AddListener(() => ChangeTab(0));
        tabButtons[1].onClick.AddListener(() => ChangeTab(1));
        tabButtons[2].onClick.AddListener(() => ChangeTab(2));

        activeTab = tabs[0];
        activeTab.SetActive(true);

    }


    void Update()
    {
        
    }

    public void ChangeTab(int tabNumber)
    {
        activeTab.SetActive(false);
        tabs[tabNumber].SetActive(true);
        activeTab = tabs[tabNumber];
    }
}
