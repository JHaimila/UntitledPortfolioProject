using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control.NPCController
{
    public class NPCTravelState : NPCBaseState
    {
        private readonly int WalkHash = Animator.StringToHash("1H_Walk_Forward");
        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        private Vector3 target;
        public NPCTravelState(NPCStateMachine stateMachine, Vector3 target) : base(stateMachine)
        {
            this.target = target;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(WalkHash, CrossFadeInFixedTime);
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
    }
}