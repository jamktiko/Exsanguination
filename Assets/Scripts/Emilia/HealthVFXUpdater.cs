using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthVFXUpdater : MonoBehaviour
{
    [SerializeField] float damageTakenVFXFlashAmount = 0.03f;

    [SerializeField] Image injuredVFXImage;
    [SerializeField] Image flashImage;
    Color flashColor;

    PlayerHealthManager healthManager;
    HealVFXHandler healVFXHandler;

    private void Awake()
    {
        healthManager = GetComponent<PlayerHealthManager>();
        healVFXHandler = GetComponent<HealVFXHandler>();

        injuredVFXImage.color = new(1, 1, 1, 0);

        flashColor = flashImage.color;
        flashColor.a = 0f;
        flashImage.color = flashColor;
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

        injuredVFXImage.color = tmpColor;
    }

    public IEnumerator FlashDamageTakenVFXCoroutine()
    {
        flashColor.a = damageTakenVFXFlashAmount;
        flashImage.color = flashColor;

        yield return new WaitForSeconds(0.1f);

        flashColor.a = 0f;
        flashImage.color = flashColor;
    }

    public void HealingVFXActivate()
    {
        healVFXHandler.StartHealAnimation();
    }
}
