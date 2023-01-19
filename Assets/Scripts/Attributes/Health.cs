using UnityEngine;
using System;
using Saving.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine.Events;
using RPG.Control;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, IAttackable, ISaveable, IChangeBehaviour
    {
        private float health = -1f;
        public event System.Action<RPG.Control.Action> HitEvent;
        public event System.Action<RPG.Control.Action> DeathEvent;
        public event System.Action<RPG.Control.Action> ReviveEvent;
        public bool isDead = false;
        
        private StateHandler stateChecker;

        public UnityEvent TakeDamage;

        private GameObject instigator;

        private BaseStats baseStats;

        private void OnEnable() 
        {
            baseStats = GetComponent<BaseStats>();
            if(health < 0)
            {
                health = baseStats.GetStat(Stat.Health);
            }
            baseStats.LevelUpEvent += SetMaxHealth;
            gameObject.TryGetComponent<StateHandler>(out stateChecker);
            if(stateChecker != null)
            {
                HitEvent += stateChecker.Check;
                DeathEvent += stateChecker.Check;
                ReviveEvent += stateChecker.Check;
            }
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
                DeathEvent?.Invoke(RPG.Control.Action.Killed);
                AwardExperience(instigator);
            }
            else
            {
                HitEvent?.Invoke(RPG.Control.Action.Attacked);
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
                stateChecker.Check(RPG.Control.Action.Killed);
                isDead = true;
                return;
            }
            else
            {
                ReviveEvent?.Invoke(RPG.Control.Action.Revived);
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
        public float GetMaxHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null){return;}

            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        public UnityAction<Control.Action> GetEvent()
        {
            throw new NotImplementedException();
        }
    }
}

