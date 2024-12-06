using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer sfxMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        // Load saved volume levels
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        ApplyVolume();
    }

    public void OnMasterVolumeChange()
    {
        float volume = Mathf.Log10(masterSlider.value) * 20;
        AudioListener.volume = masterSlider.value; // Adjust overall volume
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void OnMusicVolumeChange()
    {
        float volume = Mathf.Log10(musicSlider.value) * 20;
        musicMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void OnSFXVolumeChange()
    {
        float volume = Mathf.Log10(sfxSlider.value) * 20;
        sfxMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    private void ApplyVolume()
    {
        // Apply initial saved volumes
        OnMasterVolumeChange();
        OnMusicVolumeChange();
        OnSFXVolumeChange();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        levelManager = GameObject.Find("LevelManager")?.GetComponent<LevelManager>();
        if (scene.buildIndex != 0)
        {
            masterSlider = GameObject.Find("MasterVolumeSlider").GetComponent<Slider>();
            musicSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
            sfxSlider = GameObject.Find("SoundVolumeSlider").GetComponent<Slider>();


        }
    }
}
