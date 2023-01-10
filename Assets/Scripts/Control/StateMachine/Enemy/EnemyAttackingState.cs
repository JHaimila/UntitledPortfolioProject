using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Control.EnemyController
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        private GameObject _target;
        float previousFrameTime;

        public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Agent.isStopped = true;
            // stateMachine.LastAttack = DateTime.Now;
            stateMachine.Animator.CrossFadeInFixedTime(Animator.StringToHash(stateMachine.WeaponHandler.currentWeapon.AnimationString), CrossFadeInFixedTime);
            _target = stateMachine.Target;
        }
        public override void Tick(float deltaTime)
        {
            stateMachine.transform.LookAt(stateMachine.Target.transform, Vector3.up);
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
                // _target.transform.GetComponent<IAttackable>().OnAttack(stateMachine.WeaponHandler.currentWeapon.Damage);
                stateMachine.HandleSeesPlayer();
            }
            previousFrameTime = normalizedTime;
        }
        public override void Exit()
        {
            
        }

        
    }
}

