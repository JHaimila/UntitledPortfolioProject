using Saving.Saving;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        private float experiencePoints = -1;

        // public delegate void ExperienceGainedDelegate();
        public event Action OnExperienceGainedEvent;
        

        private BaseStats baseStats;
        
        private void Awake() {
            if(experiencePoints < 0)
            {
                baseStats = GetComponent<BaseStats>();
                experiencePoints = baseStats.GetStat(Stat.ExperienceToLevelUp, baseStats.currentLevel -1);
            }
        }

        public void GainExperience(float gainedExperience)
        {
            experiencePoints += gainedExperience;
            OnExperienceGainedEvent();
        }
        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }

        public float GetPoints()
        {
            return experiencePoints;
        }
        public float GetPointToNextLevel()
        {
            return baseStats.GetStat(Stat.ExperienceToLevelUp);
        }
        public float GetPointToLastLevel()
        {
            return baseStats.GetStat(Stat.ExperienceToLevelUp, baseStats.currentLevel -1);
        }
        
    }
}
