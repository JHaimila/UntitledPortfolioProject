using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] private Quest _giveQuest;
        public void GiveQuest()
        {
            Debug.Log("QUEST GIVEN");
            GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>().AddQuest(_giveQuest);
        }
    }
}