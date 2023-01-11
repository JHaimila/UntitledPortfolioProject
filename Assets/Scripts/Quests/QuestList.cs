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

        private QuestStatus _trackedQuest;

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

        public void TrackQuest(Quest trackQuest)
        {
            foreach(var status in statuses)
            {
                if(status.GetQuest() == trackQuest)
                {
                    if(_trackedQuest != null)
                    {
                        _trackedQuest.TrackQuest(false);
                    }
                    _trackedQuest = status;
                    _trackedQuest.TrackQuest(true);
                }
            }
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

        public bool? Evaluate(EPredicate predicate, List<string> parameters)
        {
            switch(predicate)
            {
                case EPredicate.HasQuest:
                {
                    return HasQuest(Quest.GetByName(parameters[0]));
                }
                case EPredicate.CompletedQuest:
                {
                    return GetQuestStatus(Quest.GetByName(parameters[0])).IsCompleted();
                }
                case EPredicate.CompletedObjective:
                {
                    return GetQuestStatus(Quest.GetByName(parameters[0])).GetCompleteObjectives().Contains(parameters[0]);
                }
            }
            
            return null;
        }
    }
}