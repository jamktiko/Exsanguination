using UnityEngine;
using UnityEngine.Audio;

public class MainMenuSFXFixer : MonoBehaviour
{
    public AudioMixer SFXMixer;

    // Start is called before the first frame update
    void Start()
    {
        // Set SFX volume to 0 dB (default volume level)
        if (SFXMixer != null)
        {
            SFXMixer.SetFloat("SFXVolume", 0f); // 0 dB is represented by 0 in AudioMixer
        }
        else
        {
            Debug.LogError("SFXMixer is not assigned!");
        }
    }
}
