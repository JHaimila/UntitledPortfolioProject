using System.Collections;
using System.Collections.Generic;
using InventorySystem.Inventories;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control.Routine
{
    public interface IRoutineInteract
    {
        bool Interact();
    }
}