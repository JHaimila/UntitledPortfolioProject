using UnityEngine;
using System;

namespace RPG.Control.PlayerController
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private Transform target;

        // private readonly int AttackHash = Animator.StringToHash();

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        float previousFrameTime;

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
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
            if(normalizedTime < 0.8f)
            {
                // Do Attack stuff when you set that up

                // if(normalizedTime > attack.ForceTime)
                // {
                //     TryApplyForce();
                // }
                // if(stateMachine.InputReader.IsAttacking)
                // {
                //     TryComboAttack(normalizedTime);
                // }
            }
            else
            {
                // Will have to change at some point
                // if(!stateMachine.WeaponHandler.currentWeapon.HasProjectile())
                // {
                //     _target.transform.GetComponent<IAttackable>().OnAttack(stateMachine.WeaponHandler.currentWeapon.Damage);
                // }
                stateMachine.SwitchState(new PlayerIdlingState(stateMachine));
            }
            previousFrameTime = normalizedTime;
        }
        public override void Exit()
        {
            
        }
        
    }
}

