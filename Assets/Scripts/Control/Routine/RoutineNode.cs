using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.Routine
{
    [System.Serializable]
    public class RoutineNode
    {
        [SerializeField] private RoutineType type;
        [SerializeField, Min(0)] private float waitSeconds;
        [SerializeField] private Transform point;
    }
}