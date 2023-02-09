using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem.Inventories;
using Saving.Saving;
using UnityEngine;
using RPG.Core;
using Unity.VisualScripting;

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

        private void Awake()
        {
            if (transform.TryGetComponent(out Inventory inventory))
            {
                inventory.itemAdded += CheckAddedItem;
            }
        }

        private void OnDestroy()
        {
            if (transform.TryGetComponent(out Inventory inventory))
            {
                inventory.itemAdded -= CheckAddedItem;
            }
        }

        private void CheckAddedItem(InventoryItem item)
        {
            if (HasQuest(item.GetQuest()))
            {
                CompleteObjective( item.GetQuest(),item.GetObjective());
            }
        }

        public void AddQuest(Quest quest)
        {
            if(HasQuest(quest) && !quest.IsRepeatable()){return;}
            if(HasQuest(quest) && quest.IsRepeatable())
            {
                QuestStatus status = GetQuestStatus(quest);
                status.ResetObjectives();
                return;
            }

            statuses.Add(new QuestStatus(quest));
            OnUpdate?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            if(!HasQuest(quest)){return;}
            QuestStatus status = GetQuestStatus(quest);

            if(status.IsCompleted()){return;}
            status.CompleteObjective(objective);
            OnUpdate?.Invoke();
        }
        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }
        public QuestStatus GetQuestStatus(Quest quest)
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
                    // if (parameters.Count < 2){return null;}
                    
                    return GetQuestStatus(Quest.GetByName(parameters[0])).GetCompleteObjectives().Contains(parameters[1]);
                }
            }
            
            return null;
        }
    }
}