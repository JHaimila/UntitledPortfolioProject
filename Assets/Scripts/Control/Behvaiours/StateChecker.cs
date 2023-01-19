using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Control
{
    public class StateChecker : MonoBehaviour
    {
        [SerializeField] List<StateActionMap> maps = new List<StateActionMap>();

        private BehaviourState currentBehaviour;
        public event System.Action OnBehaviourChange;
        public List<GameObject> ActionEvents;
        
        public void Check(RPG.Control.Action givenAction)
        {
            foreach(StateActionMap map in maps)
            {
                if(map.Behaviour == BehaviourState.Any && map.TriggeredBy == givenAction)
                {
                    currentBehaviour = map.TransitionsTo;
                    OnBehaviourChange?.Invoke();
                    return;
                }

                if(currentBehaviour != map.Behaviour){continue;}

                if(map.TriggeredBy != givenAction){continue;}
                // if(!map.TransitionsFromStates.Contains(currentBehaviour)){continue;}

                currentBehaviour = map.TransitionsTo;
                OnBehaviourChange?.Invoke();
                return;
            }
        }

        public BehaviourState GetCurrentBehaviour()
        {
            return currentBehaviour;
        }
        public void SetCurrentBehaviour(BehaviourState newBehaviour)
        {
            currentBehaviour = newBehaviour;
        }
    }
}
