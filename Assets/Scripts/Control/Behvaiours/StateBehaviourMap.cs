using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [System.Serializable]
    public struct StateBehaviourMap
    {
        [field:SerializeField] public BehaviourState Behaviour{get; private set;}
        [field:SerializeField] public States State{get; private set;}
    }
}