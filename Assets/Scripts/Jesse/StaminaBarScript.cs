using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float stamina;
    [SerializeField] float maxStamina;

    [SerializeField] float staminaRegenerationSpeed;
    [SerializeField] bool canRegenerateStamina;
    void Start()
    {
        slider.minValue = 0;
    }


    void Update()
    {
        UpdateStaminaBar(stamina, maxStamina);
        RegenerateStamina(staminaRegenerationSpeed);
    }

    public void UpdateStaminaBar(float stamina, float maxStamina)
    {
        slider.maxValue = maxStamina;
        slider.value = stamina;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public void UseStamina(int useAmount)
    {
        stamina -= useAmount;
    }

    public void RegenerateStamina(float regenSpeed)
    {
        if (canRegenerateStamina)
        {
            stamina = Mathf.Clamp(stamina + regenSpeed * Time.deltaTime, 0f, maxStamina);
        }
        
    }

    public void AllowStaminaRegeneration(bool a)
    {
        canRegenerateStamina = a;
    }
}
