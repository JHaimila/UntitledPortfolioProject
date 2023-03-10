using UnityEngine;
using System;

namespace RPG.Control.PlayerController
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private Transform target;

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        public PlayerAttackingState(PlayerStateMachine stateMachine, Transform target) : base(stateMachine)
        {
            this.target = target;
        }

        public override void Enter()
        {
            if(target.tag.Equals("Player")) 
            {
                stateMachine.SwitchState(new PlayerIdlingState(stateMachine));
                return;
            }
            stateMachine.Agent.isStopped = true;
            stateMachine.LastAttack = DateTime.Now;
            stateMachine.Animator.CrossFadeInFixedTime(Animator.StringToHash(stateMachine.WeaponHandler.currentWeapon.AnimationString), CrossFadeInFixedTime);
        }
        public override void Tick(float deltaTime)
        {
            stateMachine.transform.LookAt(target, Vector3.up);
            float normalizedTime = NormalizedAnimationTime(stateMachine.Animator, "Attack");
            if(normalizedTime >= 0.8f)
            {
                stateMachine.SwitchState(new PlayerIdlingState(stateMachine));
            }
        }
        public override void Exit()
        {
            stateMachine.Agent.isStopped = false;
        }
        
    }
}

