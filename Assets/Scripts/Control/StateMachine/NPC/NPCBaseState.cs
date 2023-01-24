using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.NPCController
{
    public abstract class NPCBaseState : State
    {
        protected NPCStateMachine stateMachine;
        public NPCBaseState(NPCStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}

