using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control.Routine
{
    public class PatrolNode : MonoBehaviour, IRoutineInteract
    {
        [SerializeField] private Weapon requiredWeapon;
        public Weapon GetRequiredItem()
        {
            return requiredWeapon;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public bool Interact()
        {
            return true;
        }
    }
}

