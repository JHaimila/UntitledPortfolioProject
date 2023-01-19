using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [System.Serializable, CreateAssetMenu]
    public class Behaviour:ScriptableObject
    {
        public BehaviourState behaviour;
        public bool isAggressive;
    }
}

