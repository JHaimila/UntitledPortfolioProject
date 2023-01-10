using UnityEngine;
using System;

namespace RPG.Control.PlayerController
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private RaycastHit _target;

        // private readonly int AttackHash = Animator.StringToHash();

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        float previousFrameTime;

        public PlayerAttackingState(PlayerStateMachine stateMachine, RaycastHit hit) : base(stateMachine)
        {
            this._target = hit;
        }
        public PlayerAttackingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            
        }

        public override void Enter()
        {
            if(_target.transform.gameObject == stateMachine.gameObject) 
            {
                stateMachine.SwitchState(new PlayerIdlingState(stateMachine));
                return;
            }
            stateMachine.Agent.isStopped = true;
            stateMachine.LastAttack = DateTime.Now;
            stateMachine.Animator.CrossFadeInFixedTime(Animator.StringToHash(stateMachine.WeaponHandler.currentWeapon.AnimationString), CrossFadeInFixedTime);
            // if(stateMachine.WeaponHandler.currentWeapon.HasProjectile())
            // {
            //     stateMachine.WeaponHandler.currentWeapon.LaunchProjectile(stateMachine.WeaponHandler.rightHandTransform, stateMachine.WeaponHandler.leftHandTransform, _target.transform.GetComponent<Health>());
            // }
        }
        public override void Tick(float deltaTime)
        {
            stateMachine.transform.LookAt(_target.transform, Vector3.up);
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

