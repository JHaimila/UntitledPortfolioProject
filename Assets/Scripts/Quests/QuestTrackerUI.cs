using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestTrackerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private GameObject appear;
        private QuestList questList;
        private void OnEnable()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.OnUpdate += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (!questList.HasQuest()){return;}
            
            string titleText = questList.GetCurrentQuestName();
            string descriptionText = questList.GetCurrentObjective();
            if (titleText != null && descriptionText != null)
            {
                if (!appear.activeSelf)
                {
                    appear.SetActive(true);
                }
                title.text = titleText;
                description.text = descriptionText;
                return;
            }
            appear.SetActive(false);   
        }
    }
}