using UnityEngine;

namespace RPG.Attributes
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] TMPro.TextMeshProUGUI percentText;
        private void Awake() {
            if(health == null)
            {
                health = GameObject.FindWithTag("Player").GetComponent<Health>();
            }
        }

        private void Update() {
            percentText.text = string.Format("{0:0}%",health.GetPercentage());
        }
    }
}

