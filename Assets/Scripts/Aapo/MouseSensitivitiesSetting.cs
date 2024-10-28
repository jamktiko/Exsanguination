using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivitiesSetting : MonoBehaviour
{
    private MouseLook mouseLookScript; // Reference to MouseLook script
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;


    private void Awake()
    {
        mouseLookScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>();
    }
    private void Start()
    {

        sensitivityText.text = $"Sensitivity: {sensitivitySlider.value:F1}";

        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
    }

    public void ChangeSensitivity(float newValue)
    {
        mouseLookScript.sensitivity = newValue;
        sensitivityText.text = $"Sensitivity: {newValue:F1}";
    }

   

}
