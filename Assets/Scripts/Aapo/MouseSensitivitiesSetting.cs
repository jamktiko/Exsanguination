using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivitiesSetting : MonoBehaviour
{
    [SerializeField] MouseLook mouseLookScript; // Reference to MouseLook script
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;


    
    private void Start()
    {
        if(mouseLookScript != null)
        {
            sensitivityText.text = $"Sensitivity: {mouseLookScript.sensitivity:F1}";
            sensitivitySlider.value = mouseLookScript.sensitivity;
            sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        }
        
    }

    public void ChangeSensitivity(float newValue)
    {
        if (mouseLookScript != null)
        {
            mouseLookScript.sensitivity = newValue;
            sensitivityText.text = $"Sensitivity: {newValue:F1}";
        }
    }

   

}
