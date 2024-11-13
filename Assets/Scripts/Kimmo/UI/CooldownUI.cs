using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] Slider dashBarSlider;

    public void SetDashCooldownMaxValue(float dashCooldown)
    {
        dashBarSlider.maxValue = dashCooldown;
        dashBarSlider.value = dashBarSlider.maxValue;
    }

    public void UpdateDashCooldownBar(float cooldownTimer)
    {
        dashBarSlider.value = cooldownTimer;
    }
}
