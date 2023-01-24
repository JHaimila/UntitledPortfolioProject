using RPG.Core;
using UnityEngine;

namespace RPG.Control.NPCController
{
    public class NPCChasingState : NPCBaseState
    {
        private readonly int IdleHash = Animator.StringToHash("1H_Run_Forward");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;


        public NPCChasingState(NPCStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Agent.isStopped = false;
            stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
            stateMachine.isChasing = true;
            stateMachine.Agent.speed = stateMachine.RunSpeed;
        }
        public override void Tick(float deltaTime)
        {
            
            if(stateMachine.TargetWithinRange(stateMachine.WeaponHandler.currentWeapon.Range))
            {
                if(stateMachine.Target.TryGetComponent<IAttackable>(out IAttackable target))
                {   
                    stateMachine.SwitchState(new NPCAttackingState(stateMachine));
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

