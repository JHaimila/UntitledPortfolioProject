using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Control.Routine
{
    public class Routine : MonoBehaviour
    {
        private List<RoutineNode> nodes = new List<RoutineNode>();
        [SerializeField] private bool loops = true;
        [SerializeField] private bool reverseOrder;
        private int currentIndex = 0;

        public bool Loops()
        {
            return loops;
        }

        public bool ReverseOrder()
        {
            return reverseOrder;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected() 
        {
            for(int i = 0; i < transform.childCount; i++ )
            {
                GameObject routineNode = transform.GetChild(i).gameObject;
                if(transform.GetChild(i) == transform.GetChild(0))
                {
                    Gizmos.color = Color.green;
                }
                else if(i + 1 >= transform.childCount)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.gray;
                }
                Gizmos.DrawSphere(routineNode.transform.position, 1);

                Gizmos.color = Color.gray;

                if(i+1 < transform.childCount)
                {
                    Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position);
                }
                else
                {
                    if (loops && !reverseOrder)
                    {
                        Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(0).transform.position);
                    }
                }
            }
        }
#endif
    }
}