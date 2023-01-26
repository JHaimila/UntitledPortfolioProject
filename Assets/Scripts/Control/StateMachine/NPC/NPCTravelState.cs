using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control.NPCController
{
    public class NPCTravelState : NPCBaseState
    {
        private int travelAnimationHash = Animator.StringToHash("1H_Walk_Forward");
        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        private Vector3 target;
        public NPCTravelState(NPCStateMachine stateMachine, Vector3 target) : base(stateMachine)
        {
            this.target = target;
        }
        public NPCTravelState(NPCStateMachine stateMachine, Vector3 target, string animationName) : base(stateMachine)
        {
            this.target = target;
            travelAnimationHash = Animator.StringToHash(animationName);
        }

        public override void Enter()
        {
            if (!CheckPosition(stateMachine.Agent.destination))
            {
                stateMachine.ChangeState();
                return;
            }
            stateMachine.Animator.CrossFadeInFixedTime(travelAnimationHash, CrossFadeInFixedTime);
            stateMachine.Agent.destination = target;
            stateMachine.Agent.speed = stateMachine.WalkSpeed;
        }

        public override void Tick(float deltaTime)
        {
            if(Vector3.Distance(target, stateMachine.transform.position) < 1)
            {
                stateMachine.ChangeState();
            }
        }
        
        public override void Exit()
        {
            
        }

        private bool CheckPosition(Vector3 positionToCheck)
        {
            if (NavMesh.SamplePosition(positionToCheck, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
            {
                return true;
            }
            return false;
        }
    }
}