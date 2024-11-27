using TMPro;
using UnityEngine;


public class PickUpItem : MonoBehaviour
{
    bool inArea;
    [SerializeField] public enum UtilityTool
    {
        Stake,
        GrapplingHook,
        Scroll,
        Keycard
    };

    [SerializeField] public UtilityTool tool;
    private GameObject grappleModel;
    PlayerStats playerStats;
    InputHandler inputManager;
    [SerializeField] GameObject thisScroll;
    private GameObject scrollData;
    CloseBook closeBook;
    private Light lightComponent;
    private TMP_Text text;
    private TMP_Text keyCardPickedText;
    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
        lightComponent = GetComponentInChildren<Light>();
        text = GetComponentInChildren<TMP_Text>();
        if (tool == UtilityTool.Scroll)
        {
            closeBook = GameObject.FindGameObjectWithTag("Scroll").GetComponent<CloseBook>();
            scrollData = GameObject.FindGameObjectWithTag("ScrollData");
        }
        if (tool == UtilityTool.GrapplingHook)
        {
            grappleModel = GameObject.FindGameObjectWithTag("GrappleModel");
        }
        if (tool == UtilityTool.Keycard)
        {
            keyCardPickedText = GameObject.Find("KeyCardFindUI").GetComponent<TMP_Text>();
        }
    }

    private void Start()
    {
        if (tool == UtilityTool.GrapplingHook)
        {
            grappleModel.SetActive(false);
        }
        if(tool == UtilityTool.Scroll)
        {
            scrollData.SetActive(false);
        }
        lightComponent.enabled = false;
        text.enabled = false;     
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = true;

            lightComponent.enabled = true;
            text.enabled = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        lightComponent.enabled = false;
        text.enabled = false;
        inArea = false;

    }

    public void StartPickUpItem()
    {
        if (inArea)
        {
           
            if (tool == UtilityTool.Stake)
            {
                playerStats.foundStake = true;
                gameObject.SetActive(false);
            }
            if (tool == UtilityTool.GrapplingHook)
            {
                playerStats.foundGrapplinghook = true;
                grappleModel.SetActive(true);
                gameObject.SetActive(false);
            }

            if (tool == UtilityTool.Scroll)
            {
                Debug.Log("book was clicked");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                closeBook.CloseOtherBooks();
                thisScroll.SetActive(true);
                scrollData.SetActive(true);
                Time.timeScale = 0f;               
                inputManager.DisableInput(); //how to press buttons if input is disabled on pause? Maybe instead a bool to stop moving things?
                
            }

            if(tool == UtilityTool.Keycard)
            {
                playerStats.foundKeycard = true;
                keyCardPickedText.text = "Keycard found!";
                gameObject.SetActive(false);

            }
        }
    }
 
}
