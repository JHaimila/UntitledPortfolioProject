using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public interface IChangeBehaviour
    {
        UnityEngine.Events.UnityAction<RPG.Control.Action> GetEvent();
    }
}