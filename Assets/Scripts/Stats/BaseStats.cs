using UnityEngine;
using System.Collections.Generic;
using System;
using RPG.Core;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, IPredicateEvaluator
    {
        [SerializeField, Range(1, 99)] int startingLevel = 1;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseMods = false;

        private Experience experience;

        public int currentLevel = -1;

        public event Action LevelUpEvent;

        private void Awake() 
        {
            experience = GetComponent<Experience>();
        } 
        private void OnEnable() 
        {
            currentLevel = startingLevel;
            // Debug.Log(name+" Health: "+GetStat(Stat.Health));
            if(experience != null)
            {
                experience.OnExperienceGainedEvent += UpdateLevel;
            }
        }
        private void OnDisable() 
        {
            if(experience != null)
            {
                experience.OnExperienceGainedEvent -= UpdateLevel;
            }
        }

        // private void Start() {
        //     if(gameObject.tag == "Player")
        //     {
        //         // Debug.Log("Level: ")
        //         for(int i = 1; i< 15; i++)
        //         {
        //             Debug.Log(i+ " Level Damage: "+GetStat(Stat.Damage, i));
        //         }
        //     }
        // }

        public float GetStat(Stat stat)
        {
            // return progression.GetStat(stat, characterClass, startingLevel);
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
            // return progression.FindProgressionCharacterClassWithStat(characterClass, stat).Calculate(startingLevel);
        }
        public float GetStat(Stat stat, int level)
        {
            return (GetBaseStat(stat, level) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        


        public int GetLevel()
        {
            // Experience experience = GetComponent<Experience>();
            if(experience == null){return startingLevel;}

            float currentXP = experience.GetPoints();
            
            int newLevel = currentLevel;
            // ProgressionFormula levelFormula = progression[characterClass, Stat.ExperienceToLevelUp];
            ProgressionFormula levelFormula = progression.FindProgressionWithStat(Stat.ExperienceToLevelUp);
            
            for(int i = 0; i < 99; i++)
            {
                if(currentXP < levelFormula.Calculate(i))
                {
                    newLevel = i;
                    break;
                }
            }
            return newLevel;
        }
        private void UpdateLevel()
        {
            int newLevel = GetLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEvent?.Invoke();
                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            if(levelUpParticleEffect != null)
            {
                Instantiate(levelUpParticleEffect, transform);
            }
        }
        private float GetAdditiveModifier(Stat stat)
        {
            if(!shouldUseMods){return 0;}
            float total = 0;
            IModifierProvider[] modifiers = GetComponents<IModifierProvider>();
            if(modifiers.Length == 0){return total;}

            foreach(IModifierProvider provider in modifiers)
            {
                foreach(float mod in provider.GetAdditiveModifiers(stat))
                {
                    total += mod;
                }
            }
            return total;
        }
        private float GetPercentageModifier(Stat stat)
        {
            if(!shouldUseMods){return 0;}
            float total = 0;
            IModifierProvider[] modifiers = GetComponents<IModifierProvider>();
            if(modifiers.Length == 0){return total;}

            foreach(IModifierProvider provider in modifiers)
            {
                foreach(float mod in provider.GetPercentageModifiers(stat))
                {
                    total += mod;
                }
            }
            return total;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.FindProgressionWithStat(stat).Calculate(GetLevel());
        }
        private float GetBaseStat(Stat stat, int level)
        {
            return progression.FindProgressionWithStat(stat).Calculate(level);
        }

        public bool? Evaluate(EPredicate predicate, List<string> parameters)
        {
            if(predicate == EPredicate.HasLevel)
            {
                if(int.TryParse(parameters[0], out int testLevel))
                {
                    return currentLevel >= testLevel;
                }
            }
            return null;
        }
    }
}