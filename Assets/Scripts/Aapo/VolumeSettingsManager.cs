using UnityEngine;

public class VolumeSettingsManager : MonoBehaviour
{
    public static VolumeSettingsManager Instance;

    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveVolumeSettings(float master, float music, float sfx)
    {
        masterVolume = master;
        musicVolume = music;
        sfxVolume = sfx;
    }

    public void GetVolumeSettings(out float master, out float music, out float sfx)
    {
        master = masterVolume;
        music = musicVolume;
        sfx = sfxVolume;
    }
}
