using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control.NPCController
{
    public class NPCSearchingState : NPCBaseState
    {
        public NPCSearchingState(NPCStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            if(stateMachine.searchedCount >= stateMachine.MaxSearchCount)
            {
                stateMachine.StateHandler.Check(Action.LostTarget);
                return;
            }
            Debug.Log("searching "+stateMachine.searchedCount);
            Vector3 point = GetSearchPoint();
            Debug.Log("Search point: "+point);
            stateMachine.SwitchState(new NPCTravelState(stateMachine, point));
            stateMachine.searchedCount++;
        }
        public override void Tick(float deltaTime){}
        public override void Exit()
        {
            
        }

        private Vector3 GetSearchPoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5;

            randomDirection += stateMachine.transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 5, 1);
            return hit.position;
        }
    }
}

