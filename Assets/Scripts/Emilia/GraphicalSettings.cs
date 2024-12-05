using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicalSettings : MonoBehaviour
{
    public Button previousResButton;
    public Button nextResButton;
    public Button previousQualityButton;
    public Button nextQualityButton;

    public TMP_Text resolutionText;
    public TMP_Text qualityText;

    private Resolution[] availableResolutions;
    private int currentResIndex = 0;
    private int currentQualityIndex;

    private readonly int[] classicHeights = { 480, 720, 1080, 1440, 2160 };

    [SerializeField] private SettingsSaver settingsSaver;

    private void Awake()
    {
        settingsSaver = GameObject.FindGameObjectWithTag("SettingsSaver").GetComponent<SettingsSaver>();
        currentQualityIndex = settingsSaver.GetGraphicLevel();
        currentResIndex = settingsSaver.GetResolution();
    }

    void Start()
    {
        availableResolutions = FilterResolutions(Screen.resolutions);

        if (currentResIndex < 0 || currentResIndex >= availableResolutions.Length)
        {
            currentResIndex = GetHighestResolutionIndex();
            settingsSaver.SetResolution(currentResIndex);
        }
        ApplyResolution();
        UpdateResolutionText();

        QualitySettings.SetQualityLevel(currentQualityIndex);
        UpdateQualityText();

    }

    #region Resolution
    Resolution[] FilterResolutions(Resolution[] allResolutions)
    {
        // Get the maximum supported height
        int maxSupportedHeight = Screen.currentResolution.height;

        // Filter resolutions
        return Array.FindAll(allResolutions, res =>
        {
            return Array.Exists(classicHeights, height => height == res.height) &&
                   res.width == res.height * 16 / 9 &&
                   res.height <= maxSupportedHeight;
        });
    }

    private int GetHighestResolutionIndex()
    {
        int highestIndex = 0;
        for (int i = 1; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].height > availableResolutions[highestIndex].height)
            {
                highestIndex = i;
            }
        }
        return highestIndex;
    }

    public void SelectPreviousResolution()
    {
        currentResIndex = (currentResIndex > 0) ? currentResIndex - 1 : availableResolutions.Length - 1;
        ApplyResolution();
        UpdateResolution();
    }

    public void SelectNextResolution()
    {
        currentResIndex = (currentResIndex < availableResolutions.Length - 1) ? currentResIndex + 1 : 0;
        ApplyResolution();
        UpdateResolution();
    }

    private void ApplyResolution()
    {
        Resolution selectedResolution = availableResolutions[currentResIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        UpdateResolutionText();
    }

    private void UpdateResolutionText()
    {
        Resolution selectedResolution = availableResolutions[currentResIndex];
        resolutionText.text = $"{selectedResolution.width} x {selectedResolution.height}";
    }

    private void UpdateResolution()
    {
        settingsSaver.SetResolution(currentResIndex);
    }

    #endregion

    #region Quality
    public void SelectPreviousQuality()
    {
        currentQualityIndex = (currentQualityIndex > 0) ? currentQualityIndex - 1 : QualitySettings.names.Length - 1;
        ApplyQualitySetting();
        UpdateQuality();
    }

    public void SelectNextQuality()
    {
        currentQualityIndex = (currentQualityIndex < QualitySettings.names.Length - 1) ? currentQualityIndex + 1 : 0;
        ApplyQualitySetting();
        UpdateQuality();
    }

    private void ApplyQualitySetting()
    {
        QualitySettings.SetQualityLevel(currentQualityIndex);
        UpdateQualityText();
    }

    private void UpdateQualityText()
    {
        qualityText.text = QualitySettings.names[currentQualityIndex];
    }

    private void UpdateQuality()
    {
        settingsSaver.SetGraphicLevel(currentQualityIndex);
    }

    #endregion
}