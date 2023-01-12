using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] TMPro.TextMeshProUGUI percentText;
        [SerializeField] private Image _healthBar;
        private void Awake() {
            if(health == null)
            {
                health = GameObject.FindWithTag("Player").GetComponent<Health>();
            }
        }

        private void Update() {
            if(percentText != null)
            {
                percentText.text = string.Format("{0:0}%",health.GetPercentage());
            }
            if(_healthBar != null)
            {
                _healthBar.fillAmount = NormalizeValue(0, health.GetMaxHealth(), health.GetHealth());
            }
        }
        private float NormalizeValue(float min, float max, float actual)
        {
            float top = actual - min;
            float bottom = max - min;
            return top/bottom;
        }
    }
}

