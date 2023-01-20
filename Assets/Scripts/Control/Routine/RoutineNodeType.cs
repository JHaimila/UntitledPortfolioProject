using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace RPG.Control.Routine
{
    public enum RoutineNodeType
    {
        Guard,
        WalkTo,
        PickUp,
        PutDown,
        Chop
    }
    [System.Serializable]
    public class RoutineType
    {
        public RoutineNodeType routineType;
        public AnimatorOverrideController animation;
    }
}