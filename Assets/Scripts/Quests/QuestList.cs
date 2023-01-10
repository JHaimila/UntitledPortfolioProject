using System;
using System.Collections;
using System.Collections.Generic;
using Saving.Saving;
using UnityEngine;
using RPG.Core;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        [SerializeField] List<QuestStatus> statuses;
        public event Action OnUpdate;

        public List<QuestStatus> GetStatuses()
        {
            return statuses;
        }
        public void AddQuest(Quest quest)
        {
            statuses.Add(new QuestStatus(quest));
            OnUpdate?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            status.CompleteObjective(objective);
            OnUpdate?.Invoke();
        }
        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }
        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach(QuestStatus status in statuses)
            {
                if(status.GetQuest() == quest)
                {
                    return status;
                }
            }
            return null;
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach(QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if(stateList == null){return;}

            statuses.Clear();
            foreach(object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }
        }

        public bool? Evaluate(string predicate, List<string> parameters)
        {
            if(predicate != "HasQuest"){return null;}
            switch(predicate)
            {
                case "HasQuest":
                {
                    return HasQuest(Quest.GetByName(parameters[0]));
                }
                case "DoesNotHaveQuest":
                {
                    return !HasQuest(Quest.GetByName(parameters[0]));
                }
                case "CompletedQuest":
                {
                    return GetQuestStatus(Quest.GetByName(parameters[0])).IsCompleted();
                }
            }
            
            return null;
        }
    }
}