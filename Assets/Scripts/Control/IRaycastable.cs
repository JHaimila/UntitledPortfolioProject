using System.Collections;
using System.Collections.Generic;
using RPG.Control.PlayerController;
using UnityEngine;

namespace RPG.Control
{
    public interface IRaycastable
    {
        bool HandleRaycast(InteractionHandler interactionHandler);
    }
}