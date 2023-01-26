using UnityEngine;

namespace RPG.Control.NPCController
{
    public class NPCIdlingState : NPCBaseState
    {
        // private readonly int IldingValueHash = Animator.StringToHash("Idling");
        private readonly int IdleHash = Animator.StringToHash("Idle2");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        private float duration = 0;
        private bool hasDuration = false;

        public NPCIdlingState(NPCStateMachine stateMachine) : base(stateMachine){}
        public NPCIdlingState(NPCStateMachine stateMachine, float duration) : base(stateMachine)
        {
            this.duration = duration;
            hasDuration = true;
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = true;
            stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
        }
        public override void Tick(float deltaTime)
        {
            // if(stateMachine.PlayerWithinRange(stateMachine.SightRange))
            // {
            //     stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            // }
            if(hasDuration)
            {
                if(duration <= 0)
                {
                    stateMachine.ChangeState();
                }
                duration -= deltaTime;
            }
        }
        public override void Exit()
        {
            stateMachine.Agent.isStopped = false;
        }

        
    }
}

