using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] private List<ObjectiveCompletion> objectives;
        public void CompleteObjective(int index)
        {
            if (index + 1 < objectives.Count) {return;}
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(objectives[index].quest, objectives[index].objective);
        }
    }

    [Serializable]
    class ObjectiveCompletion
    {
        public Quest quest;
        public string objective;
    }
}