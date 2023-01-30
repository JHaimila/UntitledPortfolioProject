using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.Routine
{
    public class RoutineHandler : MonoBehaviour
    {
        [SerializeField] private Routine routine;

        private List<RoutineNode> nodes = new List<RoutineNode>();

        private int currentIndex = 0;

        private void Awake()
        {
            GetNodes();
        }
        
        public Routine GetRoutine()
        {
            return routine;
        }

        public void SwitchRoutine(Routine newRoutine)
        {
            routine = newRoutine;
            currentIndex = 0;
            GetNodes();
        }

        public void GetNodes()
        {
            nodes.Clear();
            for (int i = 0; i < routine.transform.childCount; i++)
            {
                routine.transform.GetChild(i).TryGetComponent<RoutineNode>(out RoutineNode tNode);
                if (tNode != null)
                {
                    nodes.Add((tNode));
                }
            }
        }
        public void NextNode()
        {
            if(currentIndex+1 < nodes.Count)
            {
                currentIndex++;
            }
            else
            {
                if(!routine.Loops()){return;}

                if (routine.ReverseOrder())
                {
                    nodes.Reverse();
                    currentIndex = 1;
                }
                else
                {
                    currentIndex = 0;
                }
            }
        }
        public RoutineNode GetCurrentNode()
        {
            if(nodes.Count == 0){return null;}
            return nodes[currentIndex];
        }
        public bool HasNext()
        {
            if(currentIndex+1 < nodes.Count || routine.Loops()){return true;}
            return false;
        }

    }
}


