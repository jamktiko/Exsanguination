using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        // Ensure that VolumeSettingsManager is initialized before accessing it
        if (VolumeSettingsManager.Instance == null)
        {
            Debug.LogError("VolumeSettingsManager instance is missing!");
            return;
        }

        // Load saved volume settings from VolumeSettingsManager
        VolumeSettingsManager.Instance.LoadVolumeSettings();

        // Apply saved volume settings to the sliders
        if (masterSlider != null) masterSlider.value = VolumeSettingsManager.Instance.masterVolume;
        if (musicSlider != null) musicSlider.value = VolumeSettingsManager.Instance.musicVolume;
        if (sfxSlider != null) sfxSlider.value = VolumeSettingsManager.Instance.sfxVolume;

        // Apply those values to the mixers
        SetMasterVolume(VolumeSettingsManager.Instance.masterVolume);
        SetMusicVolume(VolumeSettingsManager.Instance.musicVolume);
        SetSFXVolume(VolumeSettingsManager.Instance.sfxVolume);
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        VolumeSettingsManager.Instance.SaveVolumeSettings(masterSlider.value, sliderValue, sfxSlider.value);
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        VolumeSettingsManager.Instance.SaveVolumeSettings(masterSlider.value, musicSlider.value, sliderValue);
    }

    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        VolumeSettingsManager.Instance.SaveVolumeSettings(sliderValue, musicSlider.value, sfxSlider.value);
    }
}
