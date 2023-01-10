using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName ="Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private List<ProgressionStat> stats;
        private Dictionary<Stat, ProgressionFormula> lookupTable = null;



        public ProgressionFormula FindProgressionWithStat(Stat stat)
        {
            try
            {
                MakeLookupTable();
                lookupTable.TryGetValue(stat, out ProgressionFormula formula);
                return formula;
                
            }
            catch(System.ArgumentOutOfRangeException)
            {
                throw new Exception("Couldn't find a stat");
            }
        }
        private void MakeLookupTable()
        {
            if(lookupTable != null){return;}
            lookupTable = new Dictionary<Stat, ProgressionFormula>();
            foreach(var stat in stats)
            {
                lookupTable.Add(stat.Stat, stat.Progression);
            }
        }
        [System.Serializable]
        private struct ProgressionStat
        {
            [SerializeField] private Stat stat;
            [SerializeField] private ProgressionFormula progression;
            public Stat Stat => stat;
            public ProgressionFormula Progression => progression;
        }
    }
}