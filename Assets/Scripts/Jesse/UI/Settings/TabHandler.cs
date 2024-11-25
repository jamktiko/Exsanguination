using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private Color normalColor = Color.white; // Color for normal (unselected) options
    private Color highlightColor = Color.gray; // Color for highlighted (selected) options

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

    void Update()
    {
        // Only run if the current selected object is valid
        if (eventSystem.currentSelectedGameObject != null)
        {
            // Ensure that the current selected object has at least 3 children
            Transform selectedObject = eventSystem.currentSelectedGameObject.transform;

            if (selectedObject.childCount > 2) // Check if the object has at least 3 children
            {
                // Get the third child (0-indexed), then get its first child (the arrow)
                Transform arrowTransform = selectedObject.GetChild(2).GetChild(0);

                // Check if this arrow is part of the arrowImages array
                foreach (Image arrow in arrowImages)
                {
                    if (arrow.transform == arrowTransform)
                    {
                        // Set the selected arrow to gray
                        arrow.color = highlightColor;
                    }
                    else
                    {
                        // Set all other arrows to white
                        arrow.color = normalColor;
                    }
                }
            }
        }
    }


    public void ChangeTab(int tabNumber)
    {
        activeTab.SetActive(false);
        tabs[tabNumber].SetActive(true);
        activeTab = tabs[tabNumber];
        StartCoroutine(DelaySetFirstButton(tabNumber));
    }

    private IEnumerator DelaySetFirstButton(int tabNumber)
    {
        yield return null; // Wait one frame
        inputHandler.SetFirstButton(firstOptions[tabNumber]);

    }
}
