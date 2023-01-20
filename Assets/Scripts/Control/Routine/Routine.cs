using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.Routine
{
    [System.Serializable]
    public class Routine
    {
        [SerializeField] private string RoutineName;
        [SerializeField] List<RoutineNode> nodes = new List<RoutineNode>();
        [SerializeField] private bool loops = true;
        private int currentIndex;

        public void NextNode()
        {
            
        }
    }
}