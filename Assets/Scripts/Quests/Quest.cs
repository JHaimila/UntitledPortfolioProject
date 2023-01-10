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

        [System.Serializable]
        public class Reward
        {
            public int number;
            public InventoryItem item;
        }
        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
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

        public bool HasObjective(string objectiveRef)
        {
            foreach(var objective in objectives)
            {
                if(objective.reference == objectiveRef)
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