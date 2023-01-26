using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public abstract class Pickup : MonoBehaviour, IInteractable
    {
        [field:SerializeField] public float Range{get; private set;}
        public float GetInteractRange()
        {
            return Range;
        }

        public bool IsInteractable()
        {
            return true;
        }

        public abstract void OnInteract();
    }
}

