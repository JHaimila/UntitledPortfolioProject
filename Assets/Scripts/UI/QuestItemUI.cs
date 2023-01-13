using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Quests;

namespace RPG.UI
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;
        QuestStatus status;

        public void Setup(QuestStatus status)
        {
            this.status = status;
            title.text = status.GetQuest().GetTitle();
            progress.text = status.GetCompletedCount()+"/"+status.GetQuest().GetObjectives().Count;

            if(status.IsCompleted())
            {
                title.fontStyle = FontStyles.Strikethrough;
            }

        }
    }
}