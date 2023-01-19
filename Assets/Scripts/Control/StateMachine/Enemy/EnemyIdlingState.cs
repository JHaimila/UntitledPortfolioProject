using UnityEngine;

namespace RPG.Control.EnemyController
{
    public class EnemyIdlingState : EnemyBaseState
    {
        // private readonly int IldingValueHash = Animator.StringToHash("Idling");
        private readonly int IdleHash = Animator.StringToHash("Idle2");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        private float duration = 0;
        private bool hasDuration = false;

        public EnemyIdlingState(EnemyStateMachine stateMachine) : base(stateMachine){}
        public EnemyIdlingState(EnemyStateMachine stateMachine, float duration) : base(stateMachine)
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
                    stateMachine.SetNeutralState();
                }
                duration -= deltaTime;
            }
        }
        public override void Exit()
        {
            
        }

        
    }
}

