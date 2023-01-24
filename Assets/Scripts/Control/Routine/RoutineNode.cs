using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control.Routine
{
    public class RoutineNode : MonoBehaviour
    {
        // [SerializeField] private RoutineType type;
        [SerializeField, Min(0)] private float waitSeconds;
        [SerializeField] private Transform point;
        [SerializeField] private AnimatorOverrideController nodeAnimation;
        [SerializeField] private GameObject interactObject;
        [SerializeField] private Weapon  interactItem;

        public Transform GetTransform()
        {
            if(point == null)
            {
                return transform;
            }
            return point;
        }
        public float GetWaitTime()
        {
            return waitSeconds;
        }
        public bool HasItem()
        {
            return interactItem != null;
        }
        public Weapon GetItem()
        {
            return interactItem;
        }
        public AnimatorOverrideController GetAnimation()
        {
            return nodeAnimation;
        }
        public void Interact()
        {
            if(interactObject == null){return;}
            interactObject.TryGetComponent<IRoutineInteract>(out IRoutineInteract tRoutineInteract);
            if(tRoutineInteract != null)
            {
                tRoutineInteract.Interact();
            }
        }
    }
}