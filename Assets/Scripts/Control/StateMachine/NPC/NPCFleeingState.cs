using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            Vector3 currentPosition = stateMachine.transform.position;
            Vector3 dirToTarget = currentPosition - stateMachine.Target.transform.position;
            Vector3 newPos = currentPosition + dirToTarget;
            
            NavMeshHit hit;
            
            if(NavMesh.SamplePosition(newPos, out hit, 3f, NavMesh.AllAreas))
            {
                stateMachine.SwitchState(new NPCTravelState(stateMachine, newPos, "1H_Run_Forward"));
            }
        }
    }
}


