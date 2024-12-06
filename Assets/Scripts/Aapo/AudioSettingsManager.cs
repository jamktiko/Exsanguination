using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (VolumeSettingsManager.Instance == null)
        {
            Debug.LogError("VolumeSettingsManager instance is missing!");
            return;
        }

        // Retrieve saved settings from VolumeSettingsManager
        VolumeSettingsManager.Instance.GetVolumeSettings(out float master, out float music, out float sfx);

        // Apply settings to sliders
        if (masterSlider != null)
        {
            masterSlider.value = master;
            SetMasterVolume(master);
        }

        if (musicSlider != null)
        {
            musicSlider.value = music;
            SetMusicVolume(music);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfx;
            SetSFXVolume(sfx);
        }
    }

    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        SaveCurrentSettings();
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        SaveCurrentSettings();
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        SaveCurrentSettings();
    }

    private void SaveCurrentSettings()
    {
        if (VolumeSettingsManager.Instance != null)
        {
            VolumeSettingsManager.Instance.SaveVolumeSettings(
                masterSlider != null ? masterSlider.value : 1f,
                musicSlider != null ? musicSlider.value : 1f,
                sfxSlider != null ? sfxSlider.value : 1f
            );
        }
    }
}
