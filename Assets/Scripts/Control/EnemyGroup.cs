using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace  RPG.Control
{
    public class EnemyGroup : MonoBehaviour
    {
        [SerializeField] private List<Health> enemies;
        private int numEnemies;
        public UnityEvent triggers;

        private void Awake()
        {
            foreach (var enemy in enemies)
            {
                if(enemy.isDead){continue;}
                enemy.DeathEvent += RemoveEnemy;
            }

            numEnemies = enemies.Count;
        }

        private void RemoveEnemy()
        {
            numEnemies--;
            if (numEnemies <= 0)
            {
                triggers?.Invoke();
                foreach (var enemy in enemies)
                {
                    enemy.DeathEvent -= RemoveEnemy;
                }
            }
        }
        
    }   
}