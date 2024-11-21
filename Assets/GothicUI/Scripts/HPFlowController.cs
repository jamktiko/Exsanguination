using EmiliaScripts;
using UnityEngine;
using UnityEngine.UI;

namespace CrusaderUI.Scripts
{
    public class HPFlowController : MonoBehaviour
    {
        private Material _material;
        private PlayerHealthManager playerHealthManager;
        private EnemyFinisher enemyFinisher;

        private void Awake()
        {
            playerHealthManager = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<PlayerHealthManager>();
            enemyFinisher = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<EnemyFinisher>();
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

            enemyFinisher.UpdateHealthRedness(healthPercentage);
        }
    }
}
