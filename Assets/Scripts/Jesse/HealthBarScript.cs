using EmiliaScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    Slider slider;
    TextMeshProUGUI healthText;

    GameObject player;
    PlayerHealthManager manager;

    private void Awake()
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();
        slider = GetComponent<Slider>();
        manager = GameObject.FindWithTag("HealthManager").GetComponent<PlayerHealthManager>();
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.minValue = 0;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    void GetUpdates()
    {
        UpdateHealthBar(manager.CurrentPlayerHealth(), manager.MaxPlayerHealth());
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        GetUpdates();
    }

    private void Update()
    {

    }



    private void OnEnable()
    {
        manager.OnHealthUpdate += GetUpdates;
    }
}
