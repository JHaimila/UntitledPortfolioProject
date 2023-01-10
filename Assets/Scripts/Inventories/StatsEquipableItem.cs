using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace InventorySystem.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inentory/EquipableItem"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField] Modifier[] additiveModifiers;
        [SerializeField] Modifier[] percentageModifiers;

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach(var modifier in additiveModifiers)
            {
                if(modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach(var modifier in percentageModifiers)
            {
                if(modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }
    }
}