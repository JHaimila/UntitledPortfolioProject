using System;
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
#if UNITY_EDITOR
        private Mesh indicator;
#endif
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
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (indicator == null)
            {
                indicator = Resources.Load<Mesh>("RoutineNode/SM_Prop_Arrow_01_Headless");
            }
            Gizmos.DrawWireMesh(indicator, transform.position, transform.rotation);
        }
#endif
    }

    [System.Serializable]
    public class RoutineNodeAnimation
    {
        public AnimatorOverrideController nodeAnimation;
        public float waitSeconds;
        public UnityEvent triggers;
        public Weapon interactItem;
    }

}