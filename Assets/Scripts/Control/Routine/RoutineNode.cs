using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control.Routine
{
    public class RoutineNode : MonoBehaviour
    {
        // [SerializeField] private RoutineType type;
        [SerializeField] private Transform point;
        [SerializeField] private GameObject interactObject;
        [SerializeField] private List<RoutineNodeAnimation> nodeAnimations;
        

        public Transform GetTransform()
        {
            if(!point)
            {
                return transform;
            }
            return point;
        }

        public List<RoutineNodeAnimation> GetAnimations()
        {
            return nodeAnimations;
        }

        public bool HasAnimations()
        {
            return nodeAnimations.Count > 0;
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
    [System.Serializable]
    public class RoutineNodeAnimation
    {
        public AnimatorOverrideController nodeAnimation;
        public float waitSeconds;
        public UnityEvent triggers;
        public Weapon  interactItem;
    }
}