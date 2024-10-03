using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class SettingsValueHandler : MonoBehaviour
{
    Crosshair crosshair;

    [Header("General settings")]

    [SerializeField] Toggle crosshairToggle;

    [Header("Volume settings")]

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundVolumeSlider;

    [Header("Graphics settings")]

    [SerializeField] TMP_Dropdown graphicsDropdown;

    private void Awake()
    {
        crosshair = GameObject.Find("Crosshair").GetComponent<Crosshair>();
    }

    void OnEnable()
    {
        crosshairToggle.isOn = crosshair.crosshairShown;
    }
    void Start()
    {
        crosshairToggle.onValueChanged.AddListener(delegate { ToggleCrosshair(crosshairToggle.isOn); });

        masterVolumeSlider.onValueChanged.AddListener(delegate { ChangeMasterVolume(masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(masterVolumeSlider.value); });
        soundVolumeSlider.onValueChanged.AddListener(delegate { ChangeSoundVolume(masterVolumeSlider.value); });

        graphicsDropdown.onValueChanged.AddListener(delegate { ChangeGraphics(graphicsDropdown.value); });
    }

    void ToggleCrosshair(bool value)
    {
        if (value == false)
        {
            crosshair.HideCrosshair();
        }
        else if (value == true)
        {
            crosshair.ShowCrosshair();
        }
    }

    void ChangeMasterVolume(float value)
    {
        Debug.Log($"Changed volume to: {masterVolumeSlider.value}");
        //call function from some sound script?
    }

    void ChangeMusicVolume(float value)
    {
        Debug.Log($"Changed volume to: {masterVolumeSlider.value}");
        //call function from some sound script?
    }

    void ChangeSoundVolume(float value)
    {
        Debug.Log($"Changed volume to: {masterVolumeSlider.value}");
        //call function from some sound script?
    }

    void ChangeGraphics(int index)
    {
        Debug.Log($"Graphics setting changed to: {graphicsDropdown.value}");
        //call function from graphics script?
    }
}
