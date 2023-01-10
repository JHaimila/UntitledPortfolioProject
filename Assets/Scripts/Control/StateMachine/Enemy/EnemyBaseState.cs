using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.EnemyController
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;
        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}

