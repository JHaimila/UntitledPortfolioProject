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

            stateMachine.Sight.LostTargetEvent += HandleLostPlayer;
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
            // if(!stateMachine.PlayerWithinRange(stateMachine.SightRange))
            // {
            //     stateMachine.SetPatrolling();
            //     return;
            // }
            // stateMachine.transform.LookAt(stateMachine.Player.transform, Vector3.up);
            stateMachine.Agent.destination = stateMachine.Target.transform.position;
        }
        public override void Exit()
        {
            stateMachine.isChasing = false;
            stateMachine.Sight.LostTargetEvent -= HandleLostPlayer;
        }

        public void HandleLostPlayer()
        {
            stateMachine.SwitchState(new EnemySearchingState(stateMachine));
        }
    }
}

