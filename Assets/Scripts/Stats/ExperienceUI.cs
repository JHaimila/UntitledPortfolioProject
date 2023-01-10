using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{   
    public class ExperienceUI : MonoBehaviour
    {
        [SerializeField] Experience experience  = null;
        [SerializeField] TMPro.TextMeshProUGUI percentText;
        private void Awake() {
            if(experience == null)
            {
                experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            }
        }

        private void Update() {
            percentText.text = string.Format("{0}xp",experience.GetPoints());
        }
    }
}
