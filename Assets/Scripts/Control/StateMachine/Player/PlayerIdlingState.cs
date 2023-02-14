using UnityEngine;

namespace RPG.Control.PlayerController
{
    public class PlayerIdlingState : PlayerBaseState
    {
        private readonly int IldingValueHash = Animator.StringToHash("Idling");
        private readonly int IdlingBlendTreeHash = Animator.StringToHash("IdlingBlendTree");
        
        private const float CrossFadeInFixedTime = 0.1f;
        private float idleNumber = 0;
        private float idleTransition = 2f;

        public PlayerIdlingState(PlayerStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Agent.isStopped = true;
            stateMachine.Animator.CrossFadeInFixedTime(IdlingBlendTreeHash, CrossFadeInFixedTime);
        }
        
        public override void Tick(float deltaTime)
        {
            stateMachine.Animator.SetFloat(IldingValueHash, idleNumber);
        }
        public override void Exit(){}
        
    }
}

