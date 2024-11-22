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
    [SerializeField] GameObject grappleModel;
    PlayerStats playerStats;
    InputHandler inputManager;
    [SerializeField] GameObject thisScroll;
    [SerializeField] GameObject scrollData;
    CloseBook closeBook;
    private Light lightComponent;
    private TMP_Text text;
    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
        lightComponent = GetComponentInChildren<Light>();
        text = GetComponentInChildren<TMP_Text>();
        closeBook = GameObject.FindGameObjectWithTag("Scroll").GetComponent<CloseBook>();
        grappleModel = GameObject.FindGameObjectWithTag("GrappleModel");
    }

    private void Start()
    {
        grappleModel.SetActive(false);
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
                gameObject.SetActive(false);

            }
        }
    }
 
}
