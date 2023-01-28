using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [System.Serializable]
    public struct StateActionMap
    {
        [field:SerializeField, Header("I am")] public BehaviourState Behaviour{get; private set;}
        [field:SerializeField, Header("Action happens")] public Action TriggeredBy{get; private set;}
        // [field:SerializeField, Range(1,25)] public int AmountOfActionsBeforeTransition{get; private set;}
        [field:SerializeField, Header("Then I start")] public BehaviourState TransitionsTo{get; private set;}
        // [field:SerializeField] public List<Behaviour> TransitionsFromStates{get; private set;}
        
        
    }
}
