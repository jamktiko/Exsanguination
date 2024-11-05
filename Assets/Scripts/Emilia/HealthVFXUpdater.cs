using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthVFXUpdater : MonoBehaviour
{
    [SerializeField] float damageTakenVFXFlashAmount = 0.03f;

    [SerializeField] Image injuredVFXImage;
    [SerializeField] Image hitImage;
    [SerializeField] Image hitSplatterImage;
    Color hitSplatterColor;
    Color hitColor;

    PlayerHealthManager healthManager;
    HealVFXHandler healVFXHandler;

    InjuredVFXAnimation injuredVFXAnimation;

    private void Awake()
    {
        healthManager = GetComponent<PlayerHealthManager>();
        healVFXHandler = GetComponent<HealVFXHandler>();
        injuredVFXAnimation = GetComponent<InjuredVFXAnimation>();

        injuredVFXImage.color = new(1, 1, 1, 0);

        hitColor = hitImage.color;
        hitSplatterColor = hitSplatterImage.color;
        hitColor.a = 0f;
        hitSplatterColor.a = 0f;
        hitImage.color = hitColor;
        hitSplatterImage.color = hitSplatterColor;
    }

    public void UpdateInjuryVFX(int health)
    {
        Color tmpColor = injuredVFXImage.color;
        if (health <= 90) //Threshold for updating, should match the division number
            tmpColor.a = 1 - (health / 90f); //Intensity
        else if (health > 80 && health < 100)
            tmpColor.a = 0.1f;
        else
            tmpColor.a = 0;

        if (health <= 50)
        {
            injuredVFXAnimation.StopInjuredVFXAnimation(); //Ensure effect does not stack
            injuredVFXAnimation.StartInjuredVFXAnimation(health);
        }
        else
            injuredVFXAnimation.StopInjuredVFXAnimation();

        injuredVFXImage.color = tmpColor;
    }

    public IEnumerator FlashDamageTakenVFXCoroutine()
    {
        hitColor.a = damageTakenVFXFlashAmount;
        hitImage.color = hitColor;

        hitSplatterColor.a = 1f;
        hitSplatterImage.color = hitSplatterColor;

        yield return new WaitForSeconds(0.1f);

        hitColor.a = 0f;
        hitImage.color = hitColor;

        hitSplatterColor.a = 0.5f;
        hitSplatterImage.color = hitSplatterColor;

        yield return new WaitForSeconds(0.1f);

        hitSplatterColor.a = 0f;
        hitSplatterImage.color = hitSplatterColor;
    }

    public void HealingVFXActivate()
    {
        healVFXHandler.StartHealAnimation();
    }
}
