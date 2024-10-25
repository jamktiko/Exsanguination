using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivitiesSetting : MonoBehaviour
{
    [SerializeField] private MouseLook mouseLookScript; // Reference to MouseLook script
    [SerializeField] private Slider sensitivityXSlider;
    [SerializeField] private Slider sensitivityYSlider;
    [SerializeField] private TextMeshProUGUI sensitivityXText;
    [SerializeField] private TextMeshProUGUI sensitivityYText;

    private void Start()
    {
        sensitivityXSlider.value = Mathf.Round(mouseLookScript.sensitivityX * 10f) / 10f;
        sensitivityYSlider.value = Mathf.Round(mouseLookScript.sensitivityY * 10f) / 10f;

        sensitivityXText.text = $"X Sensitivity: {sensitivityXSlider.value:F1}";
        sensitivityYText.text = $"Y Sensitivity: {sensitivityYSlider.value:F1}";

        sensitivityXSlider.onValueChanged.AddListener(ChangeSensitivityX);
        sensitivityYSlider.onValueChanged.AddListener(ChangeSensitivityY);
    }

    public void ChangeSensitivityX(float newValue)
    {
        newValue = Mathf.Round(newValue * 10f) / 10f;
        mouseLookScript.sensitivityX = newValue;
        sensitivityXText.text = $"X Sensitivity: {newValue:F1}";
    }

    public void ChangeSensitivityY(float newValue)
    {
        newValue = Mathf.Round(newValue * 10f) / 10f;
        mouseLookScript.sensitivityY = newValue;
        sensitivityYText.text = $"Y Sensitivity: {newValue:F1}";
    }

}
