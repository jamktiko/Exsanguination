using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivitiesSetting : MonoBehaviour
{
    [SerializeField] MouseLook mouseLookScript; // Reference to MouseLook script
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    private SettingsSaver settingsSaver;

    private void Awake()
    {
        settingsSaver = GameObject.FindGameObjectWithTag("SettingsSaver").GetComponent<SettingsSaver>();
        sensitivitySlider.value = settingsSaver.GetSensitivity();
        ChangeSensitivity(settingsSaver.GetSensitivity());
    }

    private void Start()
    {
        if(mouseLookScript != null)
        {
            sensitivityText.text = $"{mouseLookScript.sensitivity:F1}";
            sensitivitySlider.value = mouseLookScript.sensitivity;
            sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        }
        
    }

    public void ChangeSensitivity(float newValue)
    {
        if (mouseLookScript != null)
        {
            mouseLookScript.sensitivity = newValue;
            sensitivityText.text = $"{newValue:F1}";
            UpdateSensitivitySetting(newValue);
        }
    }

    private void UpdateSensitivitySetting(float newValue)
    {
        settingsSaver.SetSensitivity(newValue);
    }

}
