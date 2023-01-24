using UnityEngine;

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