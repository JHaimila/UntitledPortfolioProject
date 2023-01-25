using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.Routine
{
    public class Routine : MonoBehaviour
    {
        [SerializeField] List<RoutineNode> nodes = new List<RoutineNode>();
        [SerializeField] private bool loops = true;
        private int currentIndex = 0;

        public void NextNode()
        {
            if(currentIndex+1 < nodes.Count)
            {
                currentIndex++;
            }
            else
            {
                if(!loops){return;}
                currentIndex = 0;
            }
        }
        public RoutineNode GetCurrentNode()
        {
            if(nodes.Count == 0){return null;}
            return nodes[currentIndex];
        }
        public bool HasNext()
        {
            if(currentIndex+1 < nodes.Count || loops){return true;}
            return false;
        }
    }
}