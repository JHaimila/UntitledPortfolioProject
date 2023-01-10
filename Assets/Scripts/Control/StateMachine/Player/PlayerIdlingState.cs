using UnityEngine;

namespace RPG.Control.PlayerController
{
    public class PlayerIdlingState : PlayerBaseState
    {
        private readonly int IldingValueHash = Animator.StringToHash("Idling");
        private readonly int IdlingBlendTreeHash = Animator.StringToHash("IdlingBlendTree");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        float idleDuration = 5f;
        int oldIdleNum = 0;
        int newIdleNum = 0;
        float idleNumber = 0;
        float idleTransition = 2f;

        public PlayerIdlingState(PlayerStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Agent.isStopped = true;
            stateMachine.Animator.CrossFadeInFixedTime(IdlingBlendTreeHash, CrossFadeInFixedTime);
        }
        
        public override void Tick(float deltaTime)
        {
            // if(idleDuration <= 0f)
            // {
            //     ChangeAnim(deltaTime);
            //     idleDuration = 5f;
            //     return;
            // }

            stateMachine.Animator.SetFloat(IldingValueHash, idleNumber);

            // idleDuration -= deltaTime;
        }
        public override void Exit()
        {
            
        }
        
        private void ChangeAnim(float deltaTime)
        {
            oldIdleNum = newIdleNum;
            newIdleNum = (int)Random.Range(0,3);
            idleNumber = Mathf.Lerp(oldIdleNum, newIdleNum, idleTransition/deltaTime);
            
        }
        
    }
}

