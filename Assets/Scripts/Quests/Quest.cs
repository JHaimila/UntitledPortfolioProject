using System.Collections;
using System.Collections.Generic;
using InventorySystem.Inventories;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu]
    public class Quest : ScriptableObject
    {
        [SerializeField] private List<Objective> objectives = new List<Objective>();
        [SerializeField] private List<Reward>  rewards = new List<Reward>();
        [SerializeField] private bool repeatable = false;

        [System.Serializable]
        public class Reward
        {
            public int number;
            public InventoryItem item;
        }
        

        public string GetTitle()
        {
            return name;
        }
        public List<Objective> GetObjectives()
        {
            return objectives;
        }
        public List<Reward> GetRewards()
        {
            return rewards;
        }
        public bool IsRepeatable()
        {
            return repeatable;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach(var objective in objectives)
            {
                if(objective.GetReference() == objectiveRef)
                {
                    return true;
                }
            }
            return false;
        }
        public static Quest GetByName(string questName)
        {
            foreach(Quest quest in Resources.LoadAll<Quest>(""))
            {
                if(quest.name == questName)
                {
                    return quest;
                }
            }
            return null;
        }
    }
}