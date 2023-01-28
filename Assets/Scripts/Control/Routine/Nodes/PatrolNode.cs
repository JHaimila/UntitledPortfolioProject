using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control.Routine
{
    public class PatrolNode : MonoBehaviour, IRoutineInteract
    {
        [SerializeField] private Weapon requiredWeapon;
        public UnityEvent triggers;
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
            triggers?.Invoke();
            return true;
        }
    }
}

