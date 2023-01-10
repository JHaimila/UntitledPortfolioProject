using UnityEngine;

namespace RPG.Control.PlayerController
{
    public class PlayerDeathState : PlayerBaseState
    {
        private readonly int DeathHash = Animator.StringToHash("Death");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;
        public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Agent.destination = stateMachine.transform.position;
            stateMachine.Agent.isStopped = true;
            stateMachine.Animator.CrossFadeInFixedTime(DeathHash, CrossFadeInFixedTime);
            stateMachine.Health.enabled = false;
            stateMachine.enabled = false;
            stateMachine.GetComponent<CapsuleCollider>().enabled = false;
        }
        public override void Tick(float deltaTime)
        {
            
        }
        public override void Exit()
        {
            
        }

        
    }
}

