using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSaver : MonoBehaviour
{
    private string SETTINGS_SAVE_KEY_GRAPHIC_LEVEL = "graphic_level";
    private string SETTINGS_SAVE_KEY_RES = "resolution";
    private string SETTINGS_SAVE_KEY_VOLUME = "main_volume";
    private string SETTINGS_SAVE_KEY_MUSIC_VOLUME = "music_volume";
    private string SETTINGS_SAVE_KEY_SFX_VOLUME = "sfx_volume";
    private string SETTINGS_SAVE_KEY_FPS_MAX = "fps_max";
    private string SETTINGS_SAVE_KEY_SENSITIVITY = "sensitivity";

    public void SetGraphicLevel (int _value)
    {
        PlayerPrefs.SetInt(SETTINGS_SAVE_KEY_GRAPHIC_LEVEL, _value);
    }

    public int GetGraphicLevel()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SAVE_KEY_GRAPHIC_LEVEL))
        {
            return PlayerPrefs.GetInt(SETTINGS_SAVE_KEY_GRAPHIC_LEVEL, 0);
        }
        else
            return -1;
    }

    public void SetResolution(int _value)
    {
        PlayerPrefs.SetInt(SETTINGS_SAVE_KEY_GRAPHIC_LEVEL, _value);
    }

    public int GetResolution()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SAVE_KEY_RES))
        {
            return PlayerPrefs.GetInt(SETTINGS_SAVE_KEY_RES, -1);
        }
        else
            return -1;
    }

    public void SetMainVolume(float _value)
    {
        PlayerPrefs.SetFloat(SETTINGS_SAVE_KEY_VOLUME, _value);
    }

    public float GetMainVolume()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SAVE_KEY_VOLUME))
        {
            return PlayerPrefs.GetFloat(SETTINGS_SAVE_KEY_VOLUME, 100f);
        }
        else
            return 100f;
    }

    public void SetMusicVolume(float _value)
    {
        PlayerPrefs.SetFloat(SETTINGS_SAVE_KEY_MUSIC_VOLUME, _value);
    }

    public float GetMusicVolume()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SAVE_KEY_MUSIC_VOLUME))
        {
            return PlayerPrefs.GetFloat(SETTINGS_SAVE_KEY_MUSIC_VOLUME, 100f);
        }
        else
            return 100f;
    }

    public void SetSFXVolume(float _value)
    {
        PlayerPrefs.SetFloat(SETTINGS_SAVE_KEY_SFX_VOLUME, _value);
    }

    public float GetSFXVolume()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SAVE_KEY_SFX_VOLUME))
        {
            return PlayerPrefs.GetFloat(SETTINGS_SAVE_KEY_SFX_VOLUME, 100f);
        }
        else
            return 100f;
    }

    public void SetFPSMax(int _value)
    {
        PlayerPrefs.SetInt(SETTINGS_SAVE_KEY_FPS_MAX, _value);
    }

    public int GetFPSMax()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SAVE_KEY_FPS_MAX))
        {
            return PlayerPrefs.GetInt(SETTINGS_SAVE_KEY_FPS_MAX, 60);
        }
        else
            return 60;
    }

    public void SetSensitivity(float _value)
    {
        PlayerPrefs.SetFloat(SETTINGS_SAVE_KEY_SENSITIVITY, _value);
    }

    public float GetSensitivity()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SAVE_KEY_SENSITIVITY))
        {
            return PlayerPrefs.GetFloat(SETTINGS_SAVE_KEY_SENSITIVITY, 10f);
        }
        else
            return 10f;
    }

}
