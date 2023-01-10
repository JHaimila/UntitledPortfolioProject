using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public interface IInteractable
    {
        void OnInteract();
        float GetInteractRange();
    }
}