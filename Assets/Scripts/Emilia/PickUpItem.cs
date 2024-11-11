using TMPro;
using UnityEngine;


public class PickUpItem : MonoBehaviour
{
    bool inArea;
    [SerializeField] public enum UtilityTool
    {
        Stake,
        GrapplingHook,
        Scroll
    };

    [SerializeField] public UtilityTool tool;

    PlayerStats playerStats;
    InputHandler inputManager;
    [SerializeField] GameObject thisScroll;
    [SerializeField] GameObject scrollData;
    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
    }

    private void Start()
    {
        GetComponentInChildren<Light>().enabled = false;
        if (tool != UtilityTool.Scroll)
        GetComponentInChildren<TMP_Text>().enabled = false;     
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            inArea = true;

        GetComponentInChildren<Light>().enabled = true;
        if(tool != UtilityTool.Scroll)
        GetComponentInChildren<TMP_Text>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponentInChildren<Light>().enabled = false;
        if (tool != UtilityTool.Scroll)
            GetComponentInChildren<TMP_Text>().enabled = false;
    }

    public void StartPickUpItem()
    {
        if (inArea)
        {
            if (tool == UtilityTool.Stake)
                playerStats.foundStake = true;
            else if (tool == UtilityTool.GrapplingHook)
                playerStats.foundGrapplinghook = true;

            gameObject.SetActive(false);
            if(tool == UtilityTool.Scroll)
            {
                Debug.Log("book was clicked");
                gameObject.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                inputManager.DisableInput(); //how to press buttons if input is disabled on pause? Maybe instead a bool to stop moving things?
                thisScroll.SetActive(true);
                scrollData.SetActive(true);
            }
        }
    }
 
}
