using UnityEngine;

namespace RPG.Control.NPCController
{
    public class NPCDeathState : NPCBaseState
    {
        private readonly int IdleHash = Animator.StringToHash("Death");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        public NPCDeathState(NPCStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
            stateMachine.Health.enabled = false;
            stateMachine.Agent.enabled = false;
            stateMachine.GetComponent<CapsuleCollider>().enabled = false;
            stateMachine.Sight.enabled = false;
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}

