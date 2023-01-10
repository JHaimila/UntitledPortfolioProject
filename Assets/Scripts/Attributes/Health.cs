using UnityEngine;
using System;
using Saving.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, IAttackable, ISaveable
    {
        private float health = -1f;
        public event Action HitEvent;
        public event Action DeathEvent;
        public event Action ReviveEvent;
        public bool isDead = false;

        public UnityEvent TakeDamage;
        public UnityEvent Death;

        private GameObject instigator;

        private BaseStats baseStats;

        private void OnEnable() 
        {
            baseStats = GetComponent<BaseStats>();
            if(gameObject.tag == "Player")
            {
                Debug.Log(name +" Before: "+ health);
            }
            if(health < 0)
            {
                health = baseStats.GetStat(Stat.Health);
                if(gameObject.tag == "Player")
                {
                    Debug.Log(name +": "+ health);
                }
            }
            baseStats.LevelUpEvent += SetMaxHealth;
        }
        private void OnDisable() 
        {
            baseStats.LevelUpEvent -= SetMaxHealth;
        }

        public void OnAttacked(float damage, GameObject instigator)
        {
            if(health <= 0){return;}
            this.instigator = instigator;
            health = Mathf.Max(health-damage, 0);
            
            if(health == 0)
            {
                isDead = true;
                DeathEvent?.Invoke();
                Death?.Invoke();
                AwardExperience(instigator);
            }
            else
            {
                HitEvent?.Invoke();
                TakeDamage?.Invoke();
            }
        }

        

        public object CaptureState()
        {
            return health;
        }
        public void RestoreState(object state)
        {
            health = (float)state;
            if(health <= 0)
            {
                DeathEvent?.Invoke();
                isDead = true;
                return;
            }
            else
            {
                ReviveEvent?.Invoke();
                return;
            }
        }
        public float GetHealth()
        {
            return health;
        }
        public float GetPercentage()
        {
            return (health/baseStats.GetStat(Stat.Health)) * 100;
        }

        private void SetMaxHealth()
        {
            health = baseStats.GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null){return;}

            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }
    }
}

