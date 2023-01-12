using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{   
    public class ExperienceUI : MonoBehaviour
    {
        [SerializeField] Experience experience  = null;
        [SerializeField] TMPro.TextMeshProUGUI percentText;
        [SerializeField] private Image experienceBar;
        private void Awake() {
            if(experience == null)
            {
                experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
                experience.OnExperienceGainedEvent += UpdateUI;
            }
        }
        private void Start() {
            UpdateUI();
        }
        private void OnDestroy() 
        {
            experience.OnExperienceGainedEvent -= UpdateUI;
        }

        private void UpdateUI() {
            // percentText.text = string.Format("{0}xp",experience.GetPoints());
            experienceBar.fillAmount = NormalizeValue(experience.GetPointToLastLevel(), experience.GetPointToNextLevel(), experience.GetPoints());
        }
        private float NormalizeValue(float min, float max, float actual)
        {
            float top = actual - min;
            float bottom = max - min;
            return top/bottom;
        }
    }
}
