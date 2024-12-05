using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour
{
    [SerializeField] private int targetFramerate;
    [SerializeField] private TMP_Dropdown fpsDropdown;
    private int fps60 = 60;
    private int fps120 = 120;
    private int fps144 = 144;
    private int fps200 = 200;

    private SettingsSaver settingsSaver;

    private void Awake()
    {
        settingsSaver = GameObject.FindGameObjectWithTag("SettingsSaver").GetComponent<SettingsSaver>();
        targetFramerate = settingsSaver.GetFPSMax();
    }

    void Start()
    {
        Application.targetFrameRate = targetFramerate;
    }

    public void UpdateFPSMax(int fps)
    {
        settingsSaver.SetFPSMax(fps);
    }

    public void ChangeFPSTarget(int index)
    {
        switch (index)
        {
            case 0:
                Application.targetFrameRate = fps60;
                UpdateFPSMax(fps60);
                break;
            case 1:
                Application.targetFrameRate = fps120;
                UpdateFPSMax(fps120);
                break;
            case 2:
                Application.targetFrameRate = fps144;
                UpdateFPSMax(fps144);
                break;
            case 3:
                Application.targetFrameRate = fps200;
                UpdateFPSMax(fps200);
                break;
        }
    }

}
