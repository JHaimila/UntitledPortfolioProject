using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] BaseStats baseStats  = null;
        [SerializeField] TMPro.TextMeshProUGUI percentText;
        private void Awake() 
        {
            if(baseStats == null)
            {
                baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            }
            
        }
        private void Start() 
        {
            UpdateText();
        }
        private void OnEnable() 
        {
            baseStats.LevelUpEvent += UpdateText;
        }
        private void OnDisable() 
        {
            baseStats.LevelUpEvent -= UpdateText;
        }

        private void UpdateText()
        {
            percentText.text = string.Format("{0}",baseStats.GetLevel());
        }
    }
}