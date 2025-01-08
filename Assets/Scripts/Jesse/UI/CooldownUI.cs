using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] Color notReadyColor;
    [SerializeField] Color blinkColor;

    [SerializeField] bool blinking;

    [SerializeField] float fadeOutTime;
    float maxValue;

    Image cooldownImage;

    void Awake()
    {
        cooldownImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (cooldownImage.fillAmount != 1 && blinking)
        { //this is a bugfix
            blinking = false;
            StopAllCoroutines();
        }

        if (cooldownImage.fillAmount < 1)
        { //sets color to notReadyColor (white & transparent) when not full
            cooldownImage.color = notReadyColor;
        }
        if (cooldownImage.fillAmount == 1 && !blinking)
        {  //flashes blinkColor when full
            StartCoroutine(Blink());
        }
    }

    public void SetDashCooldownMaxValue(float dashCooldown)
    {
        maxValue = dashCooldown;
    }

    public void UpdateDashCooldownBar(float cooldownTimer)
    {
        cooldownImage.fillAmount = cooldownTimer / maxValue;
    }

    IEnumerator Blink()
    {
        blinking = true;

        cooldownImage.color = blinkColor;

        while (cooldownImage.color.a >= 0)
        {
            cooldownImage.color = new Color(cooldownImage.color.r, cooldownImage.color.g, cooldownImage.color.b, cooldownImage.color.a - Time.deltaTime / fadeOutTime);
            yield return null;
        }
        cooldownImage.fillAmount = 0;
        blinking = false;
    }
}