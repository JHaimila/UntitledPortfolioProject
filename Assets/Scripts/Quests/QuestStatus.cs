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
            PopulateObjectives();
        }
        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            _quest = Quest.GetByName(state.questName);
            completedObjs = state.completedObjs;
            PopulateObjectives();
        }
        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjs;
        }

        [SerializeField] private Quest _quest;
        [SerializeField] private List<ObjectiveStatus> _objectiveStatuses;
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

            foreach(var t_objective in _objectiveStatuses)
            {
                if(t_objective.GetObjective().GetReference() == objective)
                {
                    t_objective.SetCompleted(true);
                }
            }
            completedObjs.Add(objective);
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = _quest.name;
            state.completedObjs = completedObjs;
            return state;
        }

        private void PopulateObjectives()
        {
            _objectiveStatuses = new List<ObjectiveStatus>();
            foreach(var objective in _quest.GetObjectives())
            {
                _objectiveStatuses.Add(new ObjectiveStatus(objective, false, false));
            }
        }
    }
}