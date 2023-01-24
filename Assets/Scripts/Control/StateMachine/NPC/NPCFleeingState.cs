using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.NPCController
{
    public class NPCFleeingState : NPCBaseState
    {
        public NPCFleeingState(NPCStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            DeterminRunDirection();
        }
        public override void Tick(float deltaTime){}
        public override void Exit(){}

        private void DeterminRunDirection()
        {
            Vector3 dirToTarget = stateMachine.transform.position - stateMachine.Target.transform.position;

            Vector3 newPos = stateMachine.transform.position + dirToTarget;

            stateMachine.SwitchState(new NPCTravelState(stateMachine, newPos));
        }
        
    }
}


