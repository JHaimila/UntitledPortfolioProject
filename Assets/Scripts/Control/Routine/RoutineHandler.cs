using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.Routine
{
    public class RoutineHandler : MonoBehaviour
    {
        [SerializeField] private Routine routine;

        public Routine GetRoutine()
        {
            return routine;
        }

    }
}


