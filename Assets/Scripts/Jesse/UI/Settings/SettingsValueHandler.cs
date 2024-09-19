using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class SettingsValueHandler : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundVolumeSlider;

    [SerializeField] TMP_Dropdown graphicsDropdown;

    
    void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
        graphicsDropdown.onValueChanged.AddListener(delegate { ChangeGraphics(); });
    }

    void ChangeVolume()
    {
        Debug.Log($"Changed volume to: {masterVolumeSlider.value}");
        //call function from some sound script?
    }

    void ChangeGraphics()
    {
        Debug.Log($"Graphics setting changed to: {graphicsDropdown.value}");
        //call function from graphics script?
    }
}
