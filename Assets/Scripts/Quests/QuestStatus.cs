using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        public QuestStatus(Quest quest)
        {
            _quest = quest;
            completedObjs = new List<string>();
        }
        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            _quest = Quest.GetByName(state.questName);
            completedObjs = state.completedObjs;
        }
        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjs;
        }

        [SerializeField] private Quest _quest;
        [SerializeField] private List<string> completedObjs;

        public Quest GetQuest()
        {
            return _quest;
        }
        public List<string> GetCompleteObjectives()
        {
            return completedObjs;
        }
        public int GetCompletedCount()
        {
            return completedObjs.Count;
        }
        public bool IsCompleted()
        {
            if(completedObjs.Count == _quest.GetObjectives().Count)
            {
                return true;
            }
            return false;
        }

        public void CompleteObjective(string objective)
        {
            if(!_quest.HasObjective(objective)){return;}

            completedObjs.Add(objective);
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = _quest.name;
            state.completedObjs = completedObjs;
            return state;
        }
    }
}