using UnityEngine;

public class VolumeSettingsManager : MonoBehaviour
{
    public static VolumeSettingsManager Instance;

    // Stored values for master, music, and SFX volumes
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    private void Awake()
    {
        // Ensures only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy if another instance already exists
        }
    }

    public void SaveVolumeSettings(float master, float music, float sfx)
    {
        //masterVolume = master;
        musicVolume = music;
        sfxVolume = sfx;
    }

    public void LoadVolumeSettings()
    {
        
        //masterVolume = masterVolume;
        musicVolume = musicVolume ;
        sfxVolume = sfxVolume;
    }
}
