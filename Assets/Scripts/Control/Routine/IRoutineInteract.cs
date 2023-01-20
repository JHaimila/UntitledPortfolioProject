using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.Routine
{
    public interface IRoutineInteract
    {
        Transform GetTransform();
        bool Interact();
    }
}