using RPG.Core;
using UnityEngine;

namespace RPG.Control.EnemyController
{
    public class EnemyChasingState : EnemyBaseState
    {
        private readonly int IdleHash = Animator.StringToHash("1H_Run_Forward");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;


        public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Agent.isStopped = false;
            stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
            stateMachine.isChasing = true;
        }
        public override void Tick(float deltaTime)
        {
            
            if(stateMachine.PlayerWithinRange(stateMachine.WeaponHandler.currentWeapon.Range))
            {
                if(stateMachine.Target.TryGetComponent<IAttackable>(out IAttackable target))
                {   
                    stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
                }
                return;
            }
            stateMachine.Agent.destination = stateMachine.Target.transform.position;
        }
        public override void Exit()
        {
            stateMachine.isChasing = false;
        }

        public void HandleLostPlayer()
        {
            stateMachine.StateChecker.Check(Action.LostTarget);
        }
    }
}

