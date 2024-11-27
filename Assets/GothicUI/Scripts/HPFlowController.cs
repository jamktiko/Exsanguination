using EmiliaScripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CrusaderUI.Scripts
{
    public class HPFlowController : MonoBehaviour
    {
        private Material _material;
        private PlayerHealthManager playerHealthManager;
        private Volume volume;
        private ColorAdjustments colorAdjustments;

        private void Awake()
        {
            playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
            volume = GameObject.FindWithTag("FinisherVolume").GetComponent<Volume>();

            // Retrieve ColorAdjustments from the volume profile
            volume.profile.TryGet(out colorAdjustments);
        }

        private void Start()
        {
            _material = GetComponent<Image>().material;
            _material.SetFloat("_FillLevel", 1);
        }

        public void SetValue(float value)
        {
            _material.SetFloat("_FillLevel", value);
        }

        private void OnEnable()
        {
            playerHealthManager.OnHealthUpdate += OnPlayerHealthUpdate;
        }

        private void OnDisable()
        {
            playerHealthManager.OnHealthUpdate -= OnPlayerHealthUpdate;
        }

        private void OnPlayerHealthUpdate()
        {
            // Calculate the health percentage and update the material's fill level
            float healthPercentage = (float)playerHealthManager.CurrentPlayerHealth() / playerHealthManager.MaxPlayerHealth();
            SetValue(healthPercentage);

            UpdateHealthRedness(healthPercentage);
        }

        private void UpdateHealthRedness(float healthPercentage)
        {
            // Convert health percentage to a scale of 0 to 1
            float normalizedHealth = Mathf.Clamp01(healthPercentage);

            // Reduce the green and blue contribution by 50%
            float greenBlueValue = 0.5f + (normalizedHealth * 0.5f);

            // Set the color filter value, keeping red at full intensity (1f)
            colorAdjustments.colorFilter.value = new Color(1f, greenBlueValue, greenBlueValue);
        }
    }
}
